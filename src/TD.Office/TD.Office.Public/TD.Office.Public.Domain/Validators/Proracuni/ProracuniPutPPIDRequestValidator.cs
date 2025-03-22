using FluentValidation;
using LSCore.Common.Contracts;
using LSCore.Exceptions;
using LSCore.Validation.Domain;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Domain.Validators.Proracuni;

public class ProracuniPutPPIDRequestValidator : LSCoreValidatorBase<ProracuniPutPPIDRequest>
{
	public ProracuniPutPPIDRequestValidator(
		OfficeDbContext dbContext,
		ITDKomercijalnoApiManager tdKomercijalnoApiManager
	)
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.Custom(
				(id, context) =>
				{
					if (!dbContext.Proracuni.Any(x => x.Id == id))
						throw new LSCoreNotFoundException();
				}
			);

		RuleFor(x => x.PPID)
			.Custom(
				(ppid, context) =>
				{
					// Will throw 404 if PPID does not exist in komercijalno
					tdKomercijalnoApiManager
						.GetPartnerAsync(new LSCoreIdRequest() { Id = ppid!.Value })
						.GetAwaiter()
						.GetResult();
				}
			)
			.When(x => x.PPID.HasValue);
	}
}
