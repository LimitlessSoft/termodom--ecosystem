using FluentValidation;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Requests.Web;

namespace TD.Office.Public.Domain.Validators.Web;

public class CreateOrUpdateKomercijalnoPriceKoeficijentRequestValidator
	: LSCoreValidatorBase<CreateOrUpdateKomercijalnoPriceKoeficijentRequest>
{
	public CreateOrUpdateKomercijalnoPriceKoeficijentRequestValidator()
	{
		RuleFor(x => x.Naziv).NotEmpty().MaximumLength(100);

		RuleFor(x => x.Vrednost).GreaterThan(0);
	}
}
