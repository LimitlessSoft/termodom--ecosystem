using FluentValidation;
using LSCore.Domain.Validators;
using LSCore.Contracts.Exceptions;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Domain.Validators.SpecifikacijaNovca;
public class SaveSpecifikacijaNovcaRequestValidation : LSCoreValidatorBase<SaveSpecifikacijaNovcaRequest>
{
    public SaveSpecifikacijaNovcaRequestValidation(OfficeDbContext dbContext, IUserRepository userRepository)
    {
        var properties = typeof(SaveSpecifikacijaNovcaRequest).GetProperties()
        .Where(p => p.Name.StartsWith("Novcanica") && p.Name.EndsWith("Komada"));

        foreach (var property in properties)
        {
            RuleFor(x => (int?)property.GetValue(x))
                .GreaterThanOrEqualTo(0)
                .NotNull();
        }

        RuleFor(x => x)
            .Custom((specifikacija, context) =>
            {
                var user = userRepository.GetCurrentUser();
                var entity = dbContext.SpecifikacijeNovca
                    .Where(x => x.Id == specifikacija.Id).FirstOrDefault();
                if (entity == null ||
                    entity.MagacinId == user.StoreId ||
                    !user.Permissions.Any(permission =>
                        permission.IsActive &&
                        permission.Permission == Common.Contracts.Enums.Permission.SpecifikacijaNovcaSave
                ))
                    throw new LSCoreForbiddenException();
            });
    }
}
