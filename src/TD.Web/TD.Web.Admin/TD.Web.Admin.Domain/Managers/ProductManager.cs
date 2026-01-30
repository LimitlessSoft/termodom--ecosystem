using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Helpers.Products;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class ProductManager(
	IHttpContextAccessor httpContextAccessor,
	IProductRepository repository,
	IProductPriceRepository productPriceRepository,
	LSCoreAuthContextEntity<string> contextEntity,
	IProductGroupRepository productGroupRepository,
	ILogger<ProductManager> logger,
	ISettingRepository settingRepository,
	IUserManager userManager
) : IProductManager
{
	public ProductsGetDto Get(LSCoreIdRequest request)
	{
		var product = repository
			.GetMultiple()
			.Where(x => x.Id == request.Id)
			.Include(x => x.Groups)
			.Include(x => x.Unit)
			.Include(x => x.ProductPriceGroup)
			.Include(x => x.Price)
			.FirstOrDefault();

		if (product == null)
			throw new LSCoreNotFoundException();

		var dto = product.ToMapped<ProductEntity, ProductsGetDto>();
		var userCanEditAll =
			// contextUser.Id == 0 ||
			userManager.HasPermission(Permission.Admin_Products_EditAll);
		dto.CanEdit = userCanEditAll || HasPermissionToEdit(product.Id);
		return dto;
	}

	public ProductsGetDto? GetBySlug(string slug)
	{
		var product = repository
			.GetMultiple()
			.Where(x => x.Src == slug)
			.Include(x => x.Groups)
			.Include(x => x.Unit)
			.Include(x => x.ProductPriceGroup)
			.Include(x => x.Price)
			.FirstOrDefault();

		if (product == null)
			return null;

		var dto = product.ToMapped<ProductEntity, ProductsGetDto>();
		var userCanEditAll = userManager.HasPermission(Permission.Admin_Products_EditAll);
		dto.CanEdit = userCanEditAll || HasPermissionToEdit(product.Id);
		return dto;
	}

	public List<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request)
	{
		var userCanEditAll =
			httpContextAccessor.HttpContext?.User?.Identity?.AuthenticationType == "ApiKey"
			|| userManager.HasPermission(Permission.Admin_Products_EditAll);

		// Get the group IDs and individual product IDs the current user can manage
		var userManagedGroupIds = userCanEditAll
			? new List<long>()
			: userManager.GetManagingProductsGroups(contextEntity.Identifier);

		var userManagedProductIds = userCanEditAll
			? new List<long>()
			: userManager.GetManagingProducts(contextEntity.Identifier);

		// If user has no managed groups, no managed products, and can't edit all, return empty list
		if (!userCanEditAll && userManagedGroupIds.Count == 0 && userManagedProductIds.Count == 0)
			return new List<ProductsGetDto>();

		var query = repository
			.GetMultiple()
			.Where(x =>
				(
					string.IsNullOrWhiteSpace(request.SearchFilter)
					|| EF.Functions.ILike(x.Name, $"%{request.SearchFilter}%")
					|| EF.Functions.ILike(x.CatalogId, $"%{request.SearchFilter}%")
				)
				&& (request.Id == null || request.Id.Length == 0 || request.Id.Contains(x.Id))
				&& (
					request.Status == null
					|| request.Status.Length == 0
					|| request.Status.Contains(x.Status)
				)
				&& (
					request.Groups == null
					|| request.Groups.Length == 0
					|| request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))
				)
				&& (
					request.Classification == null
					|| request.Classification.Length == 0
					|| request.Classification.Any(y => y == x.Classification)
				)
				// Filter by user's managed groups OR individual products (unless they can edit all)
				&& (userCanEditAll
					|| x.Groups.Any(g => userManagedGroupIds.Contains(g.Id))
					|| userManagedProductIds.Contains(x.Id))
			);

		var products = query
			.Include(x => x.Groups)
			.ThenInclude(x => x.ManagingUsers)
			.Include(x => x.ManagingUsers)
			.Include(x => x.Unit)
			.Include(x => x.Price)
			.ToList();

		var dtoList = products.ToMappedList<ProductEntity, ProductsGetDto>();

		foreach (var dto in dtoList)
		{
			dto.CanEdit = userCanEditAll || HasPermissionToEdit(products.AsQueryable(), dto.Id);

			dto.PlatinumPriceWithoutVAT = PricesHelpers.CalculateProductPriceByLevel(
				dto.MinWebBase,
				dto.MaxWebBase,
				3
			);
			dto.IronPriceWithoutVAT = PricesHelpers.CalculateProductPriceByLevel(
				dto.MinWebBase,
				dto.MaxWebBase,
				0
			);
		}

		return dtoList;
	}

	public List<ProductsGetDto> GetSearch(ProductsGetSearchRequest request)
	{
		var userCanEditAll =
			httpContextAccessor.HttpContext?.User?.Identity?.AuthenticationType == "ApiKey"
			|| userManager.HasPermission(Permission.Admin_Products_EditAll);

		var userManagedGroupIds = userCanEditAll
			? new List<long>()
			: userManager.GetManagingProductsGroups(contextEntity.Identifier);

		var userManagedProductIds = userCanEditAll
			? new List<long>()
			: userManager.GetManagingProducts(contextEntity.Identifier);

		if (!userCanEditAll && userManagedGroupIds.Count == 0 && userManagedProductIds.Count == 0)
			return new List<ProductsGetDto>();

		return repository
			.GetMultiple()
			.Include(x => x.Groups)
			.Include(x => x.Unit)
			.Where(x =>
				(
					(
						request.Groups == null
						|| request.Groups.Length == 0
						|| request.Groups.Any(y => x.Groups.Any(z => z.Id == (int)y))
					)
					&& (
						request.Classification == null
						|| request.Classification.Length == 0
						|| request.Classification.Any(y => y == x.Classification)
					)
					&& (string.IsNullOrWhiteSpace(request.SearchTerm))
					|| EF.Functions.ILike(x.Name, $"%{request.SearchTerm}%")
					|| EF.Functions.ILike(x.CatalogId, $"%{request.SearchTerm}%")
					|| EF.Functions.ILike(x.Src, $"%{request.SearchTerm}%")
				)
				&& (userCanEditAll
					|| x.Groups.Any(g => userManagedGroupIds.Contains(g.Id))
					|| userManagedProductIds.Contains(x.Id))
			)
			.ToList()
			.ToMappedList<ProductEntity, ProductsGetDto>();
	}

	public long Save(ProductsSaveRequest request)
	{
		request.Validate();
		if (string.IsNullOrWhiteSpace(request.Src))
			request.Src = request.Name.GenerateSrc();
		var entity = request.Id is 0 or null
			? new ProductEntity()
			: repository.Get(request.Id!.Value);
		entity.InjectFrom(request);
		repository.UpdateOrInsert(entity);

		#region Update product groups

		// Get product entity since I need to include groups
		var product = repository.GetMultiple().Include(x => x.Groups).First(x => x.Id == entity.Id);

		var groups = productGroupRepository.GetMultiple().Include(x => x.ParentGroup).ToList();

		var groupToRemove = new List<long>();
		foreach (var group in request.Groups)
		{
			var qGroup = groups.FirstOrDefault(x => x.Id == group && x.IsActive);
			while (qGroup is { ParentGroup: not null })
			{
				if (request.Groups.Any(x => x == qGroup.ParentGroup.Id))
					groupToRemove.Add(qGroup.ParentGroup.Id);
				qGroup = groups.FirstOrDefault(x => x.Id == qGroup.ParentGroup.Id && x.IsActive);
			}
		}

		request.Groups.RemoveAll(x => groupToRemove.Contains(x));
		product.Groups.Clear();
		product.Groups.AddRange(groups.Where(x => request.Groups.Contains(x.Id)));

		repository.Update(product);
		#endregion

		settingRepository.SetValue(SettingKey.CACHE_HASH, DateTime.UtcNow.Ticks.ToString()); // forces cache invalidation
		return product.Id;
	}

	public List<IdNamePairDto> GetClassifications() =>
		Enum.GetValues(typeof(ProductClassification))
			.Cast<ProductClassification>()
			.Select(classification => new IdNamePairDto
			{
				Id = (int)classification,
				Name = classification.ToString()
			})
			.ToList();

	public void UpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request)
	{
		var productPrices = productPriceRepository.GetMultiple();

		foreach (var item in request.Items)
		{
			try
			{
				var productPrice =
					productPrices.FirstOrDefault(x => x.ProductId == item.ProductId)
					?? new ProductPriceEntity()
					{
						ProductId = item.ProductId,
						Min = item.MaxWebOsnova,
						Max = item.MaxWebOsnova
					};

				productPrice.Max = item.MaxWebOsnova;
				productPriceRepository.UpdateOrInsert(productPrice);
			}
			catch (Exception e)
			{
				logger.LogError(e.ToString());
			}
		}
	}

	public void UpdateMinWebOsnove(ProductsUpdateMinWebOsnoveRequest request)
	{
		var productPrices = productPriceRepository.GetMultiple();

		foreach (var item in request.Items)
		{
			var productPrice =
				productPrices.FirstOrDefault(x => x.ProductId == item.ProductId)
				?? new ProductPriceEntity()
				{
					ProductId = item.ProductId,
					Min = item.MinWebOsnova,
					Max = item.MinWebOsnova
				};

			// If min price would be greater than max price, set min price to be same as max price
			productPrice.Min =
				productPrice.Max < item.MinWebOsnova ? productPrice.Max : item.MinWebOsnova;
			productPriceRepository.Update(productPrice);
		}
	}

	public bool HasPermissionToEdit(long productId) =>
		HasPermissionToEdit(repository.GetMultiple().Include(x => x.Groups).ThenInclude(x => x.ManagingUsers).Include(x => x.ManagingUsers), productId);

	/// <summary>
	/// Checks if user has permission to edit product.
	/// Product must be in editable status AND user must be managing at least one of its groups OR the product directly.
	/// </summary>
	public bool HasPermissionToEdit(IQueryable<ProductEntity> products, long productId)
	{
		var editableStatuses = new List<ProductStatus>
		{
			ProductStatus.AzuriranjeCekaOdobrenje,
			ProductStatus.NoviCekaOdobrenje,
			ProductStatus.AzuriranjeNaObradi,
		};

		// Check if user manages product via groups
		var hasGroupAccess = products
			.Where(x => x.IsActive && x.Id == productId && editableStatuses.Contains(x.Status))
			.SelectMany(x => x.Groups.SelectMany(y => y.ManagingUsers!.Select(z => z.Username)))
			.Any(x => x == contextEntity.Identifier);

		if (hasGroupAccess)
			return true;

		// Check if user manages product directly
		return products
			.Where(x => x.IsActive && x.Id == productId && editableStatuses.Contains(x.Status))
			.SelectMany(x => x.ManagingUsers!.Select(z => z.Username))
			.Any(x => x == contextEntity.Identifier);
	}

	/// <summary>
	/// Checks if user can view/access product based on managing its groups or the product directly.
	/// </summary>
	public bool CanAccessProduct(IQueryable<ProductEntity> products, long productId)
	{
		// Check if user manages product via groups
		var hasGroupAccess = products
			.Where(x => x.IsActive && x.Id == productId)
			.SelectMany(x => x.Groups.SelectMany(y => y.ManagingUsers!.Select(z => z.Username)))
			.Any(x => x == contextEntity.Identifier);

		if (hasGroupAccess)
			return true;

		// Check if user manages product directly
		return products
			.Where(x => x.IsActive && x.Id == productId)
			.SelectMany(x => x.ManagingUsers!.Select(z => z.Username))
			.Any(x => x == contextEntity.Identifier);
	}

	public void AppendSearchKeywords(CreateProductSearchKeywordRequest request)
	{
		request.Validate();
		var product = repository.Get(request.Id);

		product.SearchKeywords ??= [];
		product.SearchKeywords!.Add(request.Keyword.ToLower());
		repository.Update(product);
		settingRepository.SetValue(SettingKey.CACHE_HASH, DateTime.UtcNow.Ticks.ToString()); // forces cache invalidation
	}

	public void DeleteSearchKeywords(DeleteProductSearchKeywordRequest request)
	{
		var product = repository.Get(request.Id);
		product.SearchKeywords?.RemoveAll(x => x.ToLower() == request.Keyword.ToLower());
		repository.Update(product);
		settingRepository.SetValue(SettingKey.CACHE_HASH, DateTime.UtcNow.Ticks.ToString()); // forces cache invalidation
	}
    public ProductsGetLinkedDto GetLinked(LSCoreIdRequest idRequest) {
        var dto = new ProductsGetLinkedDto();
        var product = repository.Get(idRequest.Id);
        dto.Link = product.Link;
        if(string.IsNullOrWhiteSpace(product.Link))
            return dto;

        var linkedProducts = repository
            .GetMultiple()
            .Where(x => x.Link == product.Link && x.Id != product.Id)
            .Select(x => $"[{x.CatalogId}] {x.Name}")
            .ToList();
        dto.LinkedProducts = linkedProducts;
        return dto;
    }
    public void SetLink(LSCoreIdRequest idRequest, string link) {
        var product = repository.Get(idRequest.Id);
        product.Link = link;
        repository.Update(product);
        settingRepository.SetValue(SettingKey.CACHE_HASH, DateTime.UtcNow.Ticks.ToString()); // forces cache invalidation
    }
}
