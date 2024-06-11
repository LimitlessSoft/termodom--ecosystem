using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.DtoMappings;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Domain.Extensions;
using TD.TDOffice.Repository;
using LSCore.Domain.Managers;
using Omu.ValueInjecter;

namespace TD.TDOffice.Domain.Managers;

public class DokumentTagIzvodManager (ILogger<DokumentTagIzvodManager> logger, TDOfficeDbContext dbContext)
    : LSCoreManagerBase<DokumentTagIzvodManager, DokumentTagIzvod>(logger, dbContext),
        IDokumentTagIzvodManager
{
    public List<DokumentTagIzvodGetDto> GetMultiple(DokumentTagIzvodGetMultipleRequest request) =>
        Queryable()
            .Where(x =>
                (!request.BrDok.HasValue || x.BrojDokumentaIzvoda == request.BrDok) &&
                (request.Korisnici == null ||  request.Korisnici.Contains(x.Korisnik)))
            .ToList()
            .ToListDto();

    public DokumentTagIzvodGetDto Save(DokumentTagizvodPutRequest request)
    {
        request.Validate();

        var entity = base.Save(request);

        var dto = new DokumentTagIzvodGetDto();
        dto.InjectFrom(entity);
        return dto;
    }
}