using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Images;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Contracts.Enums;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using Microsoft.AspNetCore.Http;
using TD.Web.Common.Contracts.Requests;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Requests.Statistics;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductManager : LSCoreBaseManager<ProductManager, ProductEntity>, IProductManager
    {
        private readonly IImageManager _imageManager;
        private readonly IOrderManager _orderManager;
        private readonly IStatisticsManager _statisticsManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext,
            IImageManager imageManager, IOrderManager orderManager,
            IHttpContextAccessor httpContextAccessor,
            IStatisticsManager statisticsManager)
            : base(logger, dbContext)
        {
            _httpContextAccessor = httpContextAccessor;

            _imageManager = imageManager;

            _orderManager = orderManager;
            
            _statisticsManager = statisticsManager;
            _statisticsManager.SetContext(_httpContextAccessor.HttpContext!);

            _orderManager.SetContext(_httpContextAccessor!.HttpContext);
        }

        public LSCoreResponse<string> AddToCart(AddToCartRequest request)
        {
            var response = new LSCoreResponse<string>();

            if (request.IsRequestInvalid(response))
                return response;

            var addResponse = _orderManager.AddItem(new OrdersAddItemRequest()
            {
                ProductId = request.Id,
                OneTimeHash = request.OneTimeHash,
                Quantity = request.Quantity,
            });
            response.Merge(addResponse);
            if (response.NotOk)
                return response;

            response.Payload = addResponse.Payload!;
            return response;
        }

        public async Task<LSCoreFileResponse> GetImageForProductAsync(ProductsGetImageRequest request)
        {
            var response = new LSCoreFileResponse();
            var query = First(x => x.IsActive && x.Src.Equals(request.Src));

            response.Merge(query);
            if (response.NotOk)
                return response;

            return await _imageManager.GetImageAsync(new ImagesGetRequest() {
                Image = query.Payload!.Image,
                Quality = request.ImageQuality ?? Constants.DefaultImageQuality,
            });
        }

        public LSCoreSortedPagedResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request)
        {
            var response = new LSCoreSortedPagedResponse<ProductsGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;
            
            if(!string.IsNullOrWhiteSpace(request.KeywordSearch))
                _statisticsManager.LogAsync(new ProductSearchKeywordRequest()
                {
                    SearchPhrase = request.KeywordSearch
                });

            var depth = 2;

            var sortedAndPagedResponse = qResponse.Payload!
                .Where(x => x.IsActive &&
                    (
                        // Group filter needs to be done manually like this
                        // Because EF Core does not support recursive queries
                        // If you increase depth, you need to add more layers (depth + 1 layers)
                        string.IsNullOrWhiteSpace(request.GroupName) ||
                        // first groups layer
                        x.Groups.Any(z => (z.Name == request.GroupName && z.IsActive) ||
                        // second groups layer
                        (z.ParentGroup != null && (z.ParentGroup.Name == request.GroupName && z.ParentGroup.IsActive)) ||
                        // third groups layer
                        (z.ParentGroup != null && z.ParentGroup.ParentGroup != null && (z.ParentGroup.ParentGroup.Name == request.GroupName && z.ParentGroup.ParentGroup.IsActive))
                )))
                .Where(x =>
                    (string.IsNullOrWhiteSpace(request.KeywordSearch) ||
                        x.Name.ToLower().Contains(request.KeywordSearch.ToLower()) ||
                        (string.IsNullOrWhiteSpace(x.CatalogId) || x.CatalogId.ToLower().Contains(request.KeywordSearch.ToLower())) ||
                        (string.IsNullOrWhiteSpace(x.ShortDescription) || x.ShortDescription.ToLower().Contains(request.KeywordSearch.ToLower()))))
                .OrderByDescending(x => x.PriorityIndex)
                .Include(x => x.Unit)
                .Include(x => x.Groups)
                .ThenIncludeRecursively(depth, x => x.ParentGroup)
                .ToSortedAndPagedResponse(request, ProductsSortColumnCodes.ProductsSortRules);

            response.Merge(sortedAndPagedResponse);
            if(response.NotOk)
                return response;

            response = new LSCoreSortedPagedResponse<ProductsGetDto>(sortedAndPagedResponse.Payload.ToDtoList<ProductsGetDto, ProductEntity>(),
                request,
                sortedAndPagedResponse.Pagination.TotalElementsCount);

            response.Payload.ForEach(async x =>
            {
                var product = sortedAndPagedResponse.Payload.FirstOrDefault(x => x.Id == x.Id);
                
                #region retrieve image
                x.ImageData = Convert.ToBase64String((await _imageManager.GetImageAsync(new ImagesGetRequest() {
                    Image = product.Image,
                    Quality = Constants.DefaultThumbnailQuality,
                })).Payload.Data);
                #endregion
                
                if (CurrentUser == null)
                {
                    var oneTimePricesResponse = ExecuteCustomQuery<GetOneTimesProductPricesRequest, OneTimePricesDto>(new GetOneTimesProductPricesRequest()
                    {
                        ProductId = x.Id
                    });
                    response.Merge(oneTimePricesResponse);
                    if (response.NotOk)
                        return;

                    x.OneTimePrice = new ProductsGetOneTimePricesDto()
                    {
                        MinPrice = oneTimePricesResponse.Payload!.MinPrice,
                        MaxPrice = oneTimePricesResponse.Payload!.MaxPrice
                    };
                }
                else
                {
                    var userPriceResponse = ExecuteCustomQuery<GetUsersProductPricesRequest, UserPricesDto>(new GetUsersProductPricesRequest()
                    {
                        ProductId = x.Id,
                        UserId = CurrentUser.Id
                    });
                    response.Merge(userPriceResponse);
                    if (response.NotOk)
                        return;

                    x.UserPrice = new ProductsGetUserPricesDto()
                    {
                        PriceWithoutVAT = userPriceResponse.Payload!.PriceWithoutVAT,
                        VAT = x.VAT
                    };
                }
            });

            return response;
        }

        public LSCoreResponse<ProductsGetSingleDto> GetSingle(ProductsGetImageRequest request)
        {
            var response = new LSCoreResponse<ProductsGetSingleDto>();

            var qResponse = Queryable(x => x.IsActive && x.Src == request.Src);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var product =
                qResponse.Payload!
                .Include(x => x.Unit)
                .Include(x => x.AlternateUnit)
                .Include(x => x.Groups)
                .Include(x => x.Price)
                .FirstOrDefault();

            if(product == null)
                return LSCoreResponse<ProductsGetSingleDto>.NotFound();

            _statisticsManager.LogAsync(new ProductViewCountRequest()
            {
                ProductId = product.Id
            });

            response.Payload = product.ToDto<ProductsGetSingleDto, ProductEntity>();
            if (CurrentUser == null)
            {
                var oneTimePricesResponse = ExecuteCustomQuery<GetOneTimesProductPricesRequest, OneTimePricesDto>(new GetOneTimesProductPricesRequest()
                {
                    ProductId = product.Id
                });
                response.Merge(oneTimePricesResponse);
                if (response.NotOk)
                    return response;

                response.Payload.OneTimePrice = new ProductsGetOneTimePricesDto()
                {
                    MinPrice = oneTimePricesResponse.Payload!.MinPrice,
                    MaxPrice = oneTimePricesResponse.Payload!.MaxPrice
                };
            }
            else
            {
                var userPriceResponse = ExecuteCustomQuery<GetUsersProductPricesRequest, UserPricesDto>(new GetUsersProductPricesRequest()
                {
                    ProductId = product.Id,
                    UserId = CurrentUser.Id
                });
                response.Merge(userPriceResponse);
                if (response.NotOk)
                    return response;

                response.Payload.UserPrice = new ProductsGetUserPricesDto()
                {
                    PriceWithoutVAT = userPriceResponse.Payload!.PriceWithoutVAT,
                    VAT = product.VAT
                };
            }

            var imageResponse = _imageManager.GetImageAsync(new ImagesGetRequest()
            {
                Image = product.Image,
                Quality = Constants.DefaultImageQuality
            }).GetAwaiter().GetResult();
            response.Merge(imageResponse);
            if (response.NotOk)
                return response;
            response.Payload.ImageData = imageResponse.Payload;

            #region Category implementation
            var categoryResponse = ExecuteCustomQuery<GetParentGroupSequentialRequest, List<GetProductGroupSequentialDto>>(new GetParentGroupSequentialRequest()
            {
                ProductId = product.Id
            });
            response.Merge(categoryResponse);
            if (response.NotOk)
                return response;
            response.Payload.Category = categoryResponse.Payload!;
            #endregion

            return response;
        }

        public LSCoreResponse RemoveFromCart(RemoveFromCartRequest request) =>
            _orderManager.RemoveItem(
                new RemoveOrderItemRequest() 
                {
                    ProductId = request.Id,
                    OneTimeHash = request.OneTimeHash
                }
            );

        public LSCoreResponse SetProductQuantity(SetCartQuantityRequest request) =>
        _orderManager.ChangeItemQuantity(
                new ChangeItemQuantityRequest()
                {
                    ProductId = request.Id,
                    OneTimeHash = request.OneTimeHash,
                    Quantity = request.Quantity
                }
            );
    }
}
