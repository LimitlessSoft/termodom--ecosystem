using FluentValidation;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Enums.ValidationCodes;
using TD.Web.Contracts.Requests.Products;
using TD.Web.Repository;
using TD.Web.Contracts.Helpers.Products;

namespace TD.Web.Domain.Validators.Products
{
    public class ProductsSaveRequestValidator : ValidatorBase<ProductsSaveRequest>
    {
        private const int NameMaximumLength = 32;
        private const int NameMinimumLength = 8;
        private const int SrcMaximumLength = 32;
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
                        context.AddFailure(ProductsValidationCodes.PVC_002.GetDescription(String.Empty));
                })
                .When(x => x.Id.HasValue);

            RuleFor(x => x)
                .Custom((request, context) =>
                {
                    var product = dbContext.Products.FirstOrDefault(x => x.Name == request.Name && x.IsActive);
                    if (product != null && product.Id != request.Id)
                        context.AddFailure(ProductsValidationCodes.PVC_001.GetDescription(String.Empty));
                });

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(NameMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ProductsSaveRequest.Name), NameMaximumLength))
                .MinimumLength(NameMinimumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_004.GetDescription(String.Empty), nameof(ProductsSaveRequest.Name), NameMinimumLength));

            RuleFor(x => x.Src)
                .NotEmpty()
                .MaximumLength(SrcMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ProductsSaveRequest.Src), SrcMaximumLength))
                .Custom((src, context) =>
                {
                    if (src.IsSrcNotValid())
                    {
                        context.AddFailure(string.Format(ProductsValidationCodes.PVC_003.GetDescription(String.Empty), nameof(ProductsSaveRequest.Src)));
                        return;
                    }

                    var checkExist = dbContext.Products.Any(x => x.Src == src && x.IsActive);
                    if (checkExist)
                        context.AddFailure(string.Format(ProductsValidationCodes.PVC_004.GetDescription(String.Empty), nameof(ProductsSaveRequest.Src)));
                });

            RuleFor(x => x.Image)
                .NotEmpty()
                .MaximumLength(ImgMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ProductsSaveRequest.Image), ImgMaximumLength));

            RuleFor(x => x.CatalogId)
                .NotEmpty()
                .MaximumLength(CatalogIdMaximumLength)
                    .WithMessage(string.Format(CommonValidationCodes.COMM_003.GetDescription(String.Empty), nameof(ProductsSaveRequest.CatalogId), CatalogIdMaximumLength));

            RuleFor(x => x.UnitId)
                .Custom((unitId, context) =>
                {
                    var unit = dbContext.Units.FirstOrDefault(x => x.Id == unitId && x.IsActive);
                    if(unit == null)
                        context.AddFailure(ProductsValidationCodes.PVC_005.GetDescription(String.Empty));
                });
            RuleFor(x => x.ProductPriceGroupId)
                .Custom((productPriceGroupId, context) =>
                {
                    var unit = dbContext.ProductPriceGroups.FirstOrDefault(x => x.Id == productPriceGroupId && x.IsActive);
                    if (unit == null)
                        context.AddFailure(ProductsValidationCodes.PVC_006.GetDescription(String.Empty));
                });
        }
    }
}
