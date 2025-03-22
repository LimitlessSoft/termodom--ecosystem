using FluentValidation;
using LSCore.Exceptions;
using LSCore.Validation.Domain;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Domain.Validators.Proracuni;

public class ProracuniPutStateRequestValidator : LSCoreValidatorBase<ProracuniPutStateRequest>
{
	public ProracuniPutStateRequestValidator(OfficeDbContext dbContext)
	{
		RuleFor(x => x.Id)
			.NotNull()
			.Custom(
				(id, context) =>
				{
					if (dbContext.Proracuni.Find(id) == null)
						throw new LSCoreNotFoundException();
				}
			);
	}
}
