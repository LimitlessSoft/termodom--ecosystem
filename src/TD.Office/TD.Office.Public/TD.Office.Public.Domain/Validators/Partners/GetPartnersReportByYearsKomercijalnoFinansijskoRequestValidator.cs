using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Public.Contracts.Requests.Partneri;

namespace TD.Office.Public.Domain.Validators.Partners
{
	public class GetPartnersReportByYearsKomercijalnoFinansijskoRequestValidator
		: LSCoreValidatorBase<GetPartnersReportByYearsKomercijalnoFinansijskoRequest>
	{
		public GetPartnersReportByYearsKomercijalnoFinansijskoRequestValidator()
		{
			RuleFor(x => x.Years).NotEmpty();
		}
	}
}
