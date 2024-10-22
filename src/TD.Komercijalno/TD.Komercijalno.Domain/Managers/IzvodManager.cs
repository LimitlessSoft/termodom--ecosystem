using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.DtoMappings.Izvodi;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers;

public class IzvodManager(ILogger<IzvodManager> logger, KomercijalnoDbContext dbContext)
    : LSCoreManagerBase<IzvodManager>(logger, dbContext), IIzvodManager
{
    public List<IzvodDto> GetMultiple(IzvodGetMultipleRequest request) =>
        dbContext.Izvodi
        .Where(x =>
            (request.PPID == null || request.PPID.Length == 0 || request.PPID.Any(z => z == x.PPID))
            && (string.IsNullOrWhiteSpace(request.PozivNaBroj) || x.PozivNaBroj == request.PozivNaBroj))
            .ToList()
            .ToIzvodiDtoList();
}
