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
using TD.Web.Common.Contracts.Dtos.Orders;
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

namespace TD.Web.Public.Domain.Managers
{
    public class ProductManager : LSCoreBaseManager<ProductManager, ProductEntity>, IProductManager
    {
        private readonly IImageManager _imageManager;
        private readonly IOrderManager _orderManager;
        private readonly IOrderItemManager _orderItemManager;

        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext,
            IImageManager imageManager, IOrderManager orderManager, IOrderItemManager orderItemManager)
            : base(logger, dbContext)
        {
            _imageManager = imageManager;
            _orderManager = orderManager;
            _orderItemManager = orderItemManager;
        }

        public LSCoreResponse AddToCart(AddToCartRequest request)
        {
            var response = new LSCoreResponse();

            if (request.IsRequestInvalid(response))
                return response;

            if (CurrentUser == null && request.OneTimeHash == String.Empty)
            {
                var hashCreator = MD5.Create();
                var hash = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(DateTime.UtcNow.ToString(Common.Contracts.Constants.UploadImageFileNameDateTimeFormatString)));
                
                foreach (byte c in hash)
                    request.OneTimeHash += $"{c:X2}";
            }

            var qResponse = Queryable(x => x.Id == request.Id && x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var product = qResponse.Payload!
                .Include(x => x.Price)
                .FirstOrDefault();

            var order = (CurrentUser == null) ? _orderManager.GetOneTimeOrder(request.OneTimeHash)?.Payload : _orderManager.GetCurrentUserOrder()?.Payload;
            
            var price = Decimal.Zero;
            if (CurrentUser != null)
            {
                var qProductPriceGroupLevelResponse = Queryable<ProductPriceGroupLevelEntity>();
                response.Merge(qProductPriceGroupLevelResponse);
                if (response.NotOk)
                    return response;

                var userLevel = qProductPriceGroupLevelResponse.Payload!
                    .FirstOrDefault(x => x.UserId == CurrentUser.Id && x.ProductPriceGroupId == product.ProductPriceGroupId && x.IsActive);

                if (userLevel == null)
                    price = product.Price.Max;
                else
                {
                    var priceDiscount = (product.Price.Max - product.Price.Min) / (Constants.NumberOfProductPriceGroupLevels - 1);
                    price = product.Price.Max - priceDiscount * userLevel.Level;
                }
            }

            if (_orderItemManager.ItemExists(product.Id, (CurrentUser == null) ? 0 : CurrentUser.Id, request.OneTimeHash))
                return LSCoreResponse.BadRequest();

            _orderItemManager.AddProductToCart(new OrderItemEntity
            {
                OrderId = order.Id,
                ProductId = request.Id,
                Quantity = request.Quantity,
                Price = price,
                PriceWithoutDiscount = product.Price.Max,
            });

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

            return new LSCoreSortedPagedResponse<ProductsGetDto>(sortedAndPagedResponse.Payload.ToDtoList<ProductsGetDto, ProductEntity>(),
                request,
                sortedAndPagedResponse.Pagination.TotalElementsCount);
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
