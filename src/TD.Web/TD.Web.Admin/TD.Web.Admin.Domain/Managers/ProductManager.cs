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
            var product = Queryable(x => x.Id == request.Id && x.IsActive)
                .Include(x => x.Groups)
                .Include(x => x.Unit)
                .Include(x => x.ProductPriceGroup)
                .FirstOrDefault();

            if (product == null)
                return LSCoreResponse<ProductsGetDto>.NotFound();

            return new LSCoreResponse<ProductsGetDto>(product.ToDto());
        }

        public LSCoreListResponse<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request) =>
            new LSCoreListResponse<ProductsGetDto>(
                Queryable(x =>
                    x.IsActive &&
                    (request.Groups == null || request.Groups.Length == 0 || request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))) &&
                    (request.Classification == null || request.Classification.Length == 0 || request.Classification.Any(y => y == x.Classification)))
                .Include(x => x.Groups)
                .Include(x => x.Unit)
                .ToList()
                .ToDtoList());

        public LSCoreListResponse<ProductsGetDto> GetSearch(ProductsGetSearchRequest request) =>
            new LSCoreListResponse<ProductsGetDto>(
                Queryable()
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

            var productEntity =
                Queryable(x => x.Id == productEntityResponse.Payload.Id)
                .Include(x => x.Groups)
                .First();

            var allGroups =
                Queryable<ProductGroupEntity>()
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
