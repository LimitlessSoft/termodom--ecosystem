using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.DtoMappings.Products;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Helpers.Products;
using TD.Web.Admin.Contracts.Interfaces.Managers;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class ProductManager : LSCoreBaseManager<ProductManager, ProductEntity>, IProductManager
    {
        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<ProductsGetDto> Get(LSCoreIdRequest request)
        {
            var response = new LSCoreResponse<ProductsGetDto>();

            var qResponse = Queryable(x => x.Id == request.Id && x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var product = qResponse.Payload!
                .Include(x => x.Groups)
                .Include(x => x.Unit)
                .Include(x => x.ProductPriceGroup)
                .Include(x => x.Price)
                .FirstOrDefault();

            if (product == null)
                return LSCoreResponse<ProductsGetDto>.NotFound();

            var price = Decimal.Zero;
            if(CurrentUser != null)
            {
                var qResponse2 = Queryable<ProductPriceGroupLevelEntity>();
                var userLevel = qResponse2.Payload!
                    .FirstOrDefault(x => x.UserId == CurrentUser.Id && x.ProductPriceGroupId == product.ProductPriceGroupId && x.IsActive);

                if (userLevel == null)
                    price = product.Price.Max;
                else
                {
                    var priceDiscount = (product.Price.Max - product.Price.Min) / (Common.Contracts.Constants.NumberOfProductPriceGroupLevels - 1);
                    price = product.Price.Max - priceDiscount * userLevel.Level;
                }
            }
            

            response.Payload = product.ToDto();
            response.Payload.Price = price;

            return response;
        }

        public LSCoreListResponse<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request)
        {
            var response = new LSCoreListResponse<ProductsGetDto>();

            var qResponse = Queryable(x =>
                    x.IsActive &&
                    (request.Groups == null || request.Groups.Length == 0 || request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))) &&
                    (request.Classification == null || request.Classification.Length == 0 || request.Classification.Any(y => y == x.Classification)));
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var products = qResponse.Payload! 
                .Include(x => x.Groups)
                .Include(x => x.Unit)
                .ToList();

            foreach (var product in products)
            {
                var price = Decimal.Zero;
                if (CurrentUser != null)
                {
                    var qResponse2 = Queryable<ProductPriceGroupLevelEntity>();
                    response.Merge(qResponse2);
                    if (response.NotOk)
                        return response;

                    var userLevel = qResponse2.Payload!
                        .FirstOrDefault(x => x.UserId == CurrentUser.Id && x.ProductPriceGroupId == product.ProductPriceGroupId && x.IsActive);

                    if (userLevel == null)
                        price = product.Price.Max;
                    else
                    {
                        var priceDiscount = (product.Price.Max - product.Price.Min) / (Common.Contracts.Constants.NumberOfProductPriceGroupLevels - 1);
                        price = product.Price.Max - priceDiscount * userLevel.Level;
                    }
                }
                var dto = product.ToDto();
                dto.Price = price;
                response.Payload.Add(dto);
            }
            return response;
        }

        public LSCoreListResponse<ProductsGetDto> GetSearch(ProductsGetSearchRequest request)
        {
            var response = new LSCoreListResponse<ProductsGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            return new LSCoreListResponse<ProductsGetDto>(
                qResponse.Payload!
                .Include(x => x.Groups)
                .Include(x => x.Unit)
                .Where(x =>
                    (request.Groups == null || request.Groups.Length == 0 || request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))) &&
                    (request.Classification == null || request.Classification.Length == 0 || request.Classification.Any(y => y == x.Classification)) &&
                    (string.IsNullOrWhiteSpace(request.SearchTerm)) ||
                        EF.Functions.ILike(x.Name, $"%{request.SearchTerm}%") ||
                        EF.Functions.ILike(x.CatalogId, $"%{request.SearchTerm}%") ||
                        EF.Functions.ILike(x.Src, $"%{request.SearchTerm}%"))
                .ToList()
                .ToDtoList());
        }

        public LSCoreResponse<long> Save(ProductsSaveRequest request)
        {
            var response = new LSCoreResponse<long>();

            if (string.IsNullOrWhiteSpace(request.Src))
                request.Src = request.Name.GenerateSrc();

            var productEntityResponse = base.Save(request);
            response.Merge(productEntityResponse);
            if (response.NotOk || productEntityResponse.Payload == null)
                return response;

            response.Payload = productEntityResponse.Payload.Id;

            #region Update product groups

            var qProductsResponse = Queryable(x => x.Id == productEntityResponse.Payload.Id);
            response.Merge(qProductsResponse);
            if (response.NotOk)
                return response;

            var productEntity =
                qProductsResponse.Payload!
                    .Include(x => x.Groups)
                    .First();

            var qGroupsResponse = Queryable<ProductGroupEntity>();
            response.Merge(qGroupsResponse);
            if (response.NotOk)
                return response;

            var allGroups =
                qGroupsResponse.Payload!
                    .ToList();

            productEntity.Groups.Clear();
            productEntity.Groups.AddRange(allGroups.Where(x => request.Groups.Contains(x.Id)));

            Update(productEntity);
            #endregion

            return response;
        }

        public LSCoreListResponse<LSCoreIdNamePairDto> GetClassifications() =>
            new LSCoreListResponse<LSCoreIdNamePairDto> (
            Enum.GetValues(typeof(ProductClassification))
                .Cast<ProductClassification>()
                .Select(classification => new LSCoreIdNamePairDto
                {
                    Id = (int)classification,
                    Name = classification.ToString()
                })
                .ToList());
    }
}
