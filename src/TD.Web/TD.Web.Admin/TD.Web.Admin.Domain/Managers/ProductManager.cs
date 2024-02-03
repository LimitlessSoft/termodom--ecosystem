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
            
            response.Payload = product.ToDto();

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
                .Include(x => x.Price)
                .ToList();

            foreach (var product in products)
            {
                var dto = product.ToDto();
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

        public LSCoreResponse UpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request)
        {
            var response = new LSCoreResponse();

            var productPriceQueryResponse = Queryable<ProductPriceEntity>();
            response.Merge(productPriceQueryResponse);
            if (response.NotOk)
                return response;

            var productPriceQuery = productPriceQueryResponse.Payload!;

            foreach (var item in request.Items)
            {
                var productPrice = productPriceQuery.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (productPrice == null)
                    productPrice = new ProductPriceEntity()
                    {
                        ProductId = item.ProductId,
                        Min = item.MaxWebOsnova,
                        Max = item.MaxWebOsnova
                    };

                productPrice.Max = item.MaxWebOsnova;
                var updateResponse = Update(productPrice);
                response.Merge(updateResponse);
                if (response.NotOk)
                    return response;
            }

            return response;
        }

        public LSCoreResponse UpdateMinWebOsnove(ProductsUpdateMinWebOsnoveRequest request)
        {
            var response = new LSCoreResponse();

            var productPriceQueryResponse = Queryable<ProductPriceEntity>();
            response.Merge(productPriceQueryResponse);
            if (response.NotOk)
                return response;

            var productPriceQuery = productPriceQueryResponse.Payload!;

            foreach (var item in request.Items)
            {
                var productPrice = productPriceQuery.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (productPrice == null)
                    productPrice = new ProductPriceEntity()
                    {
                        ProductId = item.ProductId,
                        Min = item.MinWebOsnova,
                        Max = item.MinWebOsnova
                    };

                // If min price would be greater than max price, set min price to be same as max price
                productPrice.Min = productPrice.Max < item.MinWebOsnova ? productPrice.Max : item.MinWebOsnova;
                var updateResponse = Update(productPrice);
                response.Merge(updateResponse);
                if (response.NotOk)
                    return response;
            }

            return response;
        }
    }
}
