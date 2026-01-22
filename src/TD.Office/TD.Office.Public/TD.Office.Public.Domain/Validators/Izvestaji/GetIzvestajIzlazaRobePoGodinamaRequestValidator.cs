using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Requests.Izvestaji;

namespace TD.Office.Public.Domain.Validators.Izvestaji;

public class GetIzvestajIzlazaRobePoGodinamaRequestValidator
	: LSCoreValidatorBase<GetIzvestajIzlazaRobePoGodinamaRequest>
{
	public GetIzvestajIzlazaRobePoGodinamaRequestValidator(
		IOfficeDbContextFactory dbContextFactory
	)
	{
		RuleFor(x => x.Magacin)
			.NotEmpty()
			.Custom(
				(x, context) =>
				{
					using var db = dbContextFactory.Create();
					foreach (
						var magacin in x.Where(m =>
							!db.MagacinCentri.Any(mc => mc.IsActive && mc.MagacinIds.Contains(m))
						)
					)
						context.AddFailure(
							string.Format(
								IzvestajiValidationCodes.IVC_001.GetDescription()!,
								magacin
							)
						);
				}
			);

		RuleFor(x => x.Godina)
			.NotEmpty()
			.Must(x => x.All(z => z >= 2023))
			.WithMessage(string.Format(IzvestajiValidationCodes.IVC_002.GetDescription()!, 2023));

		RuleFor(x => x.VrDok).NotEmpty();
	}
}
