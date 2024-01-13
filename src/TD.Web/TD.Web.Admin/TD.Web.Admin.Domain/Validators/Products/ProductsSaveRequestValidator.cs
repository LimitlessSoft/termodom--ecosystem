using FluentValidation;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Admin.Contracts.Helpers.Products;
using TD.Web.Common.Repository;
using LSCore.Domain.Validators;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Enums.ValidationCodes;
using JasperFx.Core;

namespace TD.Web.Admin.Domain.Validators.Products
{
    public class ProductsSaveRequestValidator : LSCoreValidatorBase<ProductsSaveRequest>
    {
        private const int NameMaximumLength = 64;
        private const int NameMinimumLength = 8;
        private const int SrcMaximumLength = 64;
        private const int ImgMaximumLength = 512;
        private const int CatalogIdMaximumLength = 16;

        public ProductsSaveRequestValidator(WebDbContext dbContext)
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Id)
                .Custom((id, context) =>
                {
                    var product = dbContext.Products.FirstOrDefault(x => x.Id == id && x.IsActive);
                    if(product == null)
                        context.AddFailure(ProductsValidationCodes.PVC_002.GetDescription());
                })
                .When(x => x.Id.HasValue);

            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var product = dbContext.Products.FirstOrDefault(x => x.Name == request.Name && x.IsActive);
                    if (product != null && product.Id != request.Id)
                        context.AddFailure(ProductsValidationCodes.PVC_001.GetDescription());

                    if (dbContext.Products.Any(x => x.Src == request.Src && x.IsActive && (request.Id == null || x.Id != request.Id)))
                        context.AddFailure(string.Format(ProductsValidationCodes.PVC_004.GetDescription(), nameof(ProductsSaveRequest.Src)));
                });

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(NameMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(ProductsSaveRequest.Name), NameMaximumLength))
                .MinimumLength(NameMinimumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_004.GetDescription(), nameof(ProductsSaveRequest.Name), NameMinimumLength));

            RuleFor(x => x.Src)
                .NotEmpty()
                .MaximumLength(SrcMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(ProductsSaveRequest.Src), SrcMaximumLength))
                .Custom((src, context) =>
                {
                    if (src.IsSrcNotValid())
                    {
                        context.AddFailure(string.Format(ProductsValidationCodes.PVC_003.GetDescription(), nameof(ProductsSaveRequest.Src)));
                        return;
                    }
                });

            RuleFor(x => x.Image)
                .NotEmpty()
                .MaximumLength(ImgMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(ProductsSaveRequest.Image), ImgMaximumLength));

            RuleFor(x => x.CatalogId)
                .NotEmpty()
                .MaximumLength(CatalogIdMaximumLength)
                    .WithMessage(string.Format(LSCoreCommonValidationCodes.COMM_003.GetDescription(), nameof(ProductsSaveRequest.CatalogId), CatalogIdMaximumLength));

            RuleFor(x => x.UnitId)
                .Custom((unitId, context) =>
                {
                    var unit = dbContext.Units.FirstOrDefault(x => x.Id == unitId && x.IsActive);
                    if(unit == null)
                        context.AddFailure(ProductsValidationCodes.PVC_005.GetDescription());
                });
            RuleFor(x => x.ProductPriceGroupId)
                .Custom((productPriceGroupId, context) =>
                {
                    var productPriceGroup = dbContext.ProductPriceGroups.FirstOrDefault(x => x.Id == productPriceGroupId && x.IsActive);
                    if (productPriceGroup == null)
                        context.AddFailure(ProductsValidationCodes.PVC_006.GetDescription());
                });
            RuleFor(x => x.CatalogId)
                .Custom((catalogId, context) =>
                {
                    if (dbContext.Products.Any(x => !catalogId.IsEmpty() && x.CatalogId == catalogId && x.IsActive))
                        context.AddFailure(ProductsValidationCodes.PVC_007.GetDescription());
                });
        }
    }
}
