using FluentValidation;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Requests.Users;

namespace TD.Web.Common.Domain.Validators.Users;

public class UsersAnalyzeOrderedProductsRequestValidator
	: LSCoreValidatorBase<UsersAnalyzeOrderedProductsRequest>
{
	public UsersAnalyzeOrderedProductsRequestValidator()
	{
		RuleFor(x => x.Range).NotNull();

		RuleFor(x => x.Username).NotEmpty();
	}
}
