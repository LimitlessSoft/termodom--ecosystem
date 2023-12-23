using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
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

        public LSCoreResponse AddToCart(AddToCartRequest request)
        {
            var response = new LSCoreResponse();

            if (request.IsRequestInvalid(response))
                return response;

            var addResponse = _orderManager.AddItem(new Common.Contracts.Requests.Orders.OrdersAddItemRequest()
            {
                ProductId = request.Id,
                OneTimeHash = request.OneTimeHash,
                Quantity = request.Quantity,
            });
            response.Merge(addResponse);
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
                Image = query.Payload.Image,
                Quality = Constants.DefaultImageQuality,
            });
        }

        public LSCoreSortedPagedResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request)
        {
            var response = new LSCoreSortedPagedResponse<ProductsGetDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var sortedAndPagedResponse = qResponse.Payload!.ToSortedAndPagedResponse(request, ProductsSortColumnCodes.ProductsSortRules);
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
                .Include(x => x.Groups)
                .FirstOrDefault();

            if(product == null)
                return LSCoreResponse<ProductsGetSingleDto>.NotFound();

            response.Payload = product.ToDto<ProductsGetSingleDto, ProductEntity>();

            var imageResponse = _imageManager.GetImageAsync(new ImagesGetRequest()
            {
                Image = product.Image,
                Quality = 1024
            }).GetAwaiter().GetResult();
            response.Merge(imageResponse);
            if (response.NotOk)
                return response;

            response.Payload.ImageData = imageResponse.Payload;
            return response;
        }
    }
}
