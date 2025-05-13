using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Helpers.Products;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Products;

public class ProductsSaveRequestValidator : LSCoreValidatorBase<ProductsSaveRequest>
{
	private const int NameMaximumLength = 64;
	private const int NameMinimumLength = 8;
	private const int SrcMaximumLength = 64;
	private const int ImgMaximumLength = 512;
	private const int CatalogIdMaximumLength = 16;

	public ProductsSaveRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		ClassLevelCascadeMode = CascadeMode.Stop;
		RuleFor(x => x.Id)
			.Custom(
				(id, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					var product = dbContext.Products.FirstOrDefault(x => x.Id == id && x.IsActive);
					if (product == null)
						context.AddFailure(ProductsValidationCodes.PVC_002.GetDescription());
				}
			)
			.When(x => x.Id.HasValue);

		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					var product = dbContext.Products.FirstOrDefault(x =>
						x.Name == request.Name && x.IsActive
					);
					if (product != null && product.Id != request.Id)
						context.AddFailure(ProductsValidationCodes.PVC_001.GetDescription());

					if (
						dbContext.Products.Any(x =>
							x.Src == request.Src
							&& x.IsActive
							&& (request.Id == null || x.Id != request.Id)
						)
					)
						context.AddFailure(
							string.Format(
								ProductsValidationCodes.PVC_004.GetDescription(),
								nameof(ProductsSaveRequest.Src)
							)
						);
				}
			);

		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(NameMaximumLength)
			.MinimumLength(NameMinimumLength);

		RuleFor(x => x.Src)
			.NotEmpty()
			.MaximumLength(SrcMaximumLength)
			.Custom(
				(src, context) =>
				{
					if (src.IsSrcNotValid())
					{
						context.AddFailure(
							string.Format(
								ProductsValidationCodes.PVC_003.GetDescription(),
								nameof(ProductsSaveRequest.Src)
							)
						);
						return;
					}
				}
			);

		RuleFor(x => x.Image).NotEmpty().MaximumLength(ImgMaximumLength);

		RuleFor(x => x.CatalogId).NotEmpty().MaximumLength(CatalogIdMaximumLength);

		RuleFor(x => x.UnitId)
			.Custom(
				(unitId, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					var unit = dbContext.Units.FirstOrDefault(x => x.Id == unitId && x.IsActive);
					if (unit == null)
						context.AddFailure(ProductsValidationCodes.PVC_005.GetDescription());
				}
			);

		RuleFor(x => x.ProductPriceGroupId)
			.Custom(
				(productPriceGroupId, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					var productPriceGroup = dbContext.ProductPriceGroups.FirstOrDefault(x =>
						x.Id == productPriceGroupId && x.IsActive
					);
					if (productPriceGroup == null)
						context.AddFailure(ProductsValidationCodes.PVC_006.GetDescription());
				}
			);

		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					if (
						dbContext.Products.Any(x =>
							request.CatalogId != null
							&& x.CatalogId!.ToUpper() == request.CatalogId.ToUpper()
							&& x.IsActive
							&& (request.Id == null || request.Id != x.Id)
						)
					)
						context.AddFailure(ProductsValidationCodes.PVC_007.GetDescription());
				}
			);

		RuleFor(x => x.Groups)
			.NotEmpty()
			.Must(x => x.Count > 0)
			.WithMessage(ProductsValidationCodes.PVC_008.GetDescription());

		RuleFor(x => x.Description)
			.Custom(
				(description, context) =>
				{
					var invalidTags = ProductsHelpers.FindUnwantedHtmlTags(description!);
					if (invalidTags.Count > 0)
						context.AddFailure(
							string.Format(
								ProductsValidationCodes.PVC_010.GetDescription(),
								string.Join(", ", invalidTags)
							)
						);
				}
			)
			.When(x => !string.IsNullOrWhiteSpace(x.Description));
	}
}
