using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.DtoMappings.Products;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Enums;
using TD.Web.Contracts.Helpers.Products;
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

        public Response<ProductsGetDto> Get(IdRequest request)
        {
            var product = Queryable()
                .Where(x => x.Id == request.Id)
                .Include(x => x.Groups)
                .Include(x => x.Unit)
                .FirstOrDefault();

            if (product == null)
                return Response<ProductsGetDto>.NotFound();

            return new Response<ProductsGetDto>(product.ToDto());
        }

        public ListResponse<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request) =>
            new ListResponse<ProductsGetDto>(
                Queryable()
                .Include(x => x.Groups)
                .Include(x => x.Unit)
                .Where(x =>
                    (request.Groups == null || request.Groups.Length == 0 || request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))) &&
                    (request.Classification == null || request.Classification.Length == 0 || request.Classification.Any(y => y == x.Classification)))
                .ToList()
                .ToDtoList());

        public ListResponse<ProductsGetDto> GetSearch(ProductsGetSearchRequest request) =>
            new ListResponse<ProductsGetDto>(
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

        public Response<long> Save(ProductsSaveRequest request)
        {
            var response = new Response<long>();

            if (string.IsNullOrWhiteSpace(request.Src))
                request.Src = request.Name.GenerateSrc();

            if (request.IsRequestInvalid(response))
                return response;

            var productEntityResponse = base.Save(request);
            response.Merge(productEntityResponse);
            if (response.NotOk || productEntityResponse.Payload == null)
                return response;

            response.Payload = productEntityResponse.Payload.Id;

            #region Update product groups
            productEntityResponse.Payload.Groups = Queryable<ProductGroupEntity>()
                .Where(x => request.Groups.Contains(x.Id))
                .ToList();

            Update(productEntityResponse.Payload);
            #endregion

            return response;
        }

        public ListResponse<ProductsClassificationsDto> GetClassifications() =>
            new ListResponse<ProductsClassificationsDto> (
            Enum.GetValues(typeof(ProductClassification))
                .Cast<ProductClassification>()
                .Select(classification => new ProductsClassificationsDto
                {
                    Id = (int)classification,
                    Name = classification.ToString()
                })
                .ToList());
    }
}
