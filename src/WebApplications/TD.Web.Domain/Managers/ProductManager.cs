using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.DtoMappings.Products;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.Managers;
using TD.Web.Contracts.Requests.Products;
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
{
    public class ProductManager : BaseManager<ProductManager, ProductEntity>, IProductManager
    {
        public ProductManager(ILogger<ProductManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public ListResponse<ProductsGetMultipleDto> GetMultiple(ProductsGetMultipleRequest request) =>
            new ListResponse<ProductsGetMultipleDto>(
                Queryable()
                .Include(x => x.Groups)
                .Where(x =>
                    (request.Groups == null || request.Groups.Length == 0 || request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))) &&
                    (request.Classification == null || request.Classification.Length == 0 || request.Classification.Any(y => y == x.Classification)))
                .ToList()
                .ToDtoList());

        public ListResponse<ProductsGetMultipleDto> GetSearch(ProductsGetSearchRequest request) =>
            new ListResponse<ProductsGetMultipleDto>(
                Queryable()
                .Include(x => x.Groups)
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
}
