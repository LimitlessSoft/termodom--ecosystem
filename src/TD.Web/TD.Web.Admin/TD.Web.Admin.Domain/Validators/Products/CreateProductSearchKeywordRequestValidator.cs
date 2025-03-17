using FluentValidation;
using FluentValidation.Results;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Products;

public class CreateProductSearchKeywordRequestValidator
	: LSCoreValidatorBase<CreateProductSearchKeywordRequest>
{
	public CreateProductSearchKeywordRequestValidator(WebDbContext dbContext)
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.Keyword).NotEmpty();
		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					var product = dbContext.Products.FirstOrDefault(x => x.Id == request.Id);

					if (product == null)
						throw new LSCoreNotFoundException();

					if (product.SearchKeywords == null)
						return;

					if (product.SearchKeywords!.Any(x => x.ToLower() == request.Keyword.ToLower()))
						context.AddFailure(
							new ValidationFailure(
								request.Keyword,
								ProductsValidationCodes.PVC_009.GetDescription()
							)
						);
				}
			);
	}
}
