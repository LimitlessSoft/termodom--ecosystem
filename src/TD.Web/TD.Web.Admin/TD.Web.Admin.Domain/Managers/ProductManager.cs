using LSCore.Contracts;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Admin.Contracts.Helpers.Products;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Enums;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Contracts.Dtos;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Domain.Managers;

public class ProductManager (ILogger<ProductManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser, IUserManager userManager)
    : LSCoreManagerBase<ProductManager, ProductEntity>(logger, dbContext, contextUser), IProductManager
{
    public ProductsGetDto Get(LSCoreIdRequest request)
    {
        var product = Queryable()
            .Where(x => x.Id == request.Id && x.IsActive)
            .Include(x => x.Groups)
            .Include(x => x.Unit)
            .Include(x => x.ProductPriceGroup)
            .Include(x => x.Price)
            .FirstOrDefault();

        if (product == null)
            throw new LSCoreNotFoundException();

        var dto = product.ToDto<ProductEntity, ProductsGetDto>();
        var userCanEditAll = CurrentUser?.Id == 0 ||userManager.HasPermission(Permission.Admin_Products_EditAll);
        dto.CanEdit = userCanEditAll || HasPermissionToEdit(product.Id);
        return dto;
    }

    public List<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request)
    {
        var query = Queryable()
            .Where(x =>
                x.IsActive &&
                (string.IsNullOrWhiteSpace(request.SearchFilter) ||
                 EF.Functions.ILike(x.Name, $"%{request.SearchFilter}%") ||
                 EF.Functions.ILike(x.CatalogId, $"%{request.SearchFilter}%")) &&
                (request.Id == null || request.Id.Length == 0 || request.Id.Contains(x.Id)) &&
                (request.Status == null || request.Status.Length == 0 || request.Status.Contains(x.Status)) &&
                (request.Groups == null || request.Groups.Length == 0 || request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))) &&
                (request.Classification == null || request.Classification.Length == 0 || request.Classification.Any(y => y == x.Classification)));

        var products = query
            .Include(x => x.Groups)
            .ThenInclude(x => x.ManagingUsers)
            .Include(x => x.Unit)
            .Include(x => x.Price)
            .ToList();

        var dtoList = products.ToDtoList<ProductEntity, ProductsGetDto>();
        var userCanEditAll = CurrentUser?.Id == 0 || userManager.HasPermission(Permission.Admin_Products_EditAll);
        
        foreach(var dto in dtoList)
            dto.CanEdit = userCanEditAll || HasPermissionToEdit(products.AsQueryable(), dto.Id);
        
        return dtoList.Where(x => x.CanEdit).ToList();
    }

    public List<ProductsGetDto> GetSearch(ProductsGetSearchRequest request) =>
        Queryable()
            .Where(x => x.IsActive)
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
            .ToDtoList<ProductEntity, ProductsGetDto>();

    public long Save(ProductsSaveRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Src))
            request.Src = request.Name.GenerateSrc();

        var productEntityResponse = base.Save(request);

        #region Update product groups

        // Get product entity since I need to include groups
        var product = Queryable()
            .Include(x => x.Groups)
            .First(x => x.Id == productEntityResponse.Id);

        var groups = Queryable<ProductGroupEntity>()
            .Include(x => x.ParentGroup)
            .ToList();
            
        var groupToRemove = new List<long>();
        foreach (var group in request.Groups)
        {
            var qGroup = groups.FirstOrDefault(x => x.Id == group && x.IsActive);
            while(qGroup is { ParentGroup: not null })
            {
                if (request.Groups.Any(x => x == qGroup.ParentGroup.Id))
                    groupToRemove.Add(qGroup.ParentGroup.Id);
                qGroup = groups.FirstOrDefault(x => x.Id == qGroup.ParentGroup.Id && x.IsActive);
            }
        }

        request.Groups.RemoveAll(x => groupToRemove.Contains(x));
        product.Groups.Clear();
        product.Groups.AddRange(groups.Where(x => request.Groups.Contains(x.Id)));

        Update(product);
        #endregion

        return product.Id;
    }

    public List<LSCoreIdNamePairDto> GetClassifications() =>
        Enum.GetValues(typeof(ProductClassification))
            .Cast<ProductClassification>()
            .Select(classification => new LSCoreIdNamePairDto
            {
                Id = (int)classification,
                Name = classification.ToString()
            })
            .ToList();

    public void UpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request)
    {
        var productPrices = Queryable<ProductPriceEntity>();

        foreach (var item in request.Items)
        {
            var productPrice = productPrices.FirstOrDefault(x => x.ProductId == item.ProductId) ?? new ProductPriceEntity()
            {
                ProductId = item.ProductId,
                Min = item.MaxWebOsnova,
                Max = item.MaxWebOsnova
            };

            productPrice.Max = item.MaxWebOsnova;
            Update(productPrice);
        }
    }

    public void UpdateMinWebOsnove(ProductsUpdateMinWebOsnoveRequest request)
    {
        var productPrices = Queryable<ProductPriceEntity>();

        foreach (var item in request.Items)
        {
            var productPrice = productPrices.FirstOrDefault(x => x.ProductId == item.ProductId) ?? new ProductPriceEntity()
            {
                ProductId = item.ProductId,
                Min = item.MinWebOsnova,
                Max = item.MinWebOsnova
            };

            // If min price would be greater than max price, set min price to be same as max price
            productPrice.Min = productPrice.Max < item.MinWebOsnova ? productPrice.Max : item.MinWebOsnova;
            Update(productPrice);
        }
    }

    public bool HasPermissionToEdit(long productId) =>
        HasPermissionToEdit(Queryable().Include(x => x.Groups), productId);

    /// <summary>
    /// Checks if user has permission to edit product.
    /// </summary>
    /// <param name="products"></param>
    /// <param name="productId"></param>
    /// <returns></returns>
    public bool HasPermissionToEdit(IQueryable<ProductEntity> products, long productId) =>
        products.Where(x => x.IsActive
                && x.Id == productId
                && new List<ProductStatus>
                {
                    ProductStatus.AzuriranjeCekaOdobrenje,
                    ProductStatus.NoviCekaOdobrenje,
                    ProductStatus.AzuriranjeNaObradi,
                    ProductStatus.AzuriranjeCekaOdobrenje
                }.Contains(x.Status))
            .SelectMany(x => x.Groups.SelectMany(y => y.ManagingUsers!.Select(z => z.Id)))
            .Any(x => x == CurrentUser!.Id);

    public void AppendSearchKeywords(CreateProductSearchKeywordRequest request)
    {
        request.Validate();
        var product = Queryable()
            .FirstOrDefault(x => x.Id == request.Id);
        
        if(product == null)
            throw new LSCoreNotFoundException();

        product.SearchKeywords ??= [];
        product.SearchKeywords!.Add(request.Keyword.ToLower());
        
        Update(product);
    }

    public void DeleteSearchKeywords(DeleteProductSearchKeywordRequest request)
    {
        var product = Queryable()
            .FirstOrDefault(x => x.Id == request.Id);

        if(product == null)
            throw new LSCoreNotFoundException();
        
        product.SearchKeywords?.RemoveAll(x => x.ToLower() == request.Keyword.ToLower());
        Update(product);
    }
}