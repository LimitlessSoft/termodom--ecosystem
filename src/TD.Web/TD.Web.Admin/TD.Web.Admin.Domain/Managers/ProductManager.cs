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

	public List<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request)
	{
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
			);

		var products = query
			.Include(x => x.Groups)
			.ThenInclude(x => x.ManagingUsers)
			.Include(x => x.Unit)
			.Include(x => x.Price)
			.ToList();

		var dtoList = products.ToMappedList<ProductEntity, ProductsGetDto>();
		var userCanEditAll =
			httpContextAccessor.HttpContext?.User?.Identity?.AuthenticationType == "ApiKey"
			// || contextUser.Id == 0
			|| userManager.HasPermission(Permission.Admin_Products_EditAll);

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

		return dtoList.Where(x => x.CanEdit).ToList();
	}

	public List<ProductsGetDto> GetSearch(ProductsGetSearchRequest request) =>
		repository
			.GetMultiple()
			.Include(x => x.Groups)
			.Include(x => x.Unit)
			.Where(x =>
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
			.ToList()
			.ToMappedList<ProductEntity, ProductsGetDto>();

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
		HasPermissionToEdit(repository.GetMultiple().Include(x => x.Groups), productId);

	/// <summary>
	/// Checks if user has permission to edit product.
	/// </summary>
	/// <param name="products"></param>
	/// <param name="productId"></param>
	/// <returns></returns>
	public bool HasPermissionToEdit(IQueryable<ProductEntity> products, long productId) =>
		products
			.Where(x =>
				x.IsActive
				&& x.Id == productId
				&& new List<ProductStatus>
				{
					ProductStatus.AzuriranjeCekaOdobrenje,
					ProductStatus.NoviCekaOdobrenje,
					ProductStatus.AzuriranjeNaObradi,
					ProductStatus.AzuriranjeCekaOdobrenje
				}.Contains(x.Status)
			)
			.SelectMany(x => x.Groups.SelectMany(y => y.ManagingUsers!.Select(z => z.Username)))
			.Any(x => x == contextEntity.Identifier);

	public void AppendSearchKeywords(CreateProductSearchKeywordRequest request)
	{
		request.Validate();
		var product = repository.Get(request.Id);

		product.SearchKeywords ??= [];
		product.SearchKeywords!.Add(request.Keyword.ToLower());
		repository.Update(product);
	}

	public void DeleteSearchKeywords(DeleteProductSearchKeywordRequest request)
	{
		var product = repository.Get(request.Id);
		product.SearchKeywords?.RemoveAll(x => x.ToLower() == request.Keyword.ToLower());
		repository.Update(product);
	}
}
