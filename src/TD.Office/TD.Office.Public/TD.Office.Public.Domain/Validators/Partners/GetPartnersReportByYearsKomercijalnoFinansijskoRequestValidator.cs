using FluentValidation;
using LSCore.Domain.Validators;
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
