using FluentValidation;
using LSCore.Domain.Validators;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Domain.Validators.SpecifikacijaNovca;
public class SaveSpecifikacijaNovcaRequestValidation : LSCoreValidatorBase<SaveSpecifikacijaNovcaRequest>
{
    public SaveSpecifikacijaNovcaRequestValidation()
    {
        var properties = typeof(SaveSpecifikacijaNovcaRequest).GetProperties()
        .Where(p => p.Name.StartsWith("Novcanica") && p.Name.EndsWith("Komada"));

        foreach (var property in properties)
        {
            RuleFor(x => (int?)property.GetValue(x))
                .GreaterThanOrEqualTo(0)
                .NotNull();
        }

    }
}
