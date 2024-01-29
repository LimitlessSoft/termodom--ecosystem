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
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.Products;
using Microsoft.AspNetCore.Http;
using TD.Web.Common.Contracts.Requests;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Helpers;
using System.Linq.Expressions;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductManager : LSCoreBaseManager<ProductManager, ProductEntity>, IProductManager
    {
        private readonly IImageManager _imageManager;
        private readonly IOrderManager _orderManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext,
            IImageManager imageManager, IOrderManager orderManager,
            IHttpContextAccessor httpContextAccessor)
            : base(logger, dbContext)
        {
            _httpContextAccessor = httpContextAccessor;

            _imageManager = imageManager;

            _orderManager = orderManager;

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

            response.Payload.ForEach(x =>
            {
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
