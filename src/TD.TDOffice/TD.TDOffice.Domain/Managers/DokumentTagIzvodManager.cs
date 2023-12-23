using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.TDOffice.Contracts.DtoMappings;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers
{
    public class DokumentTagIzvodManager : LSCoreBaseManager<DokumentTagIzvodManager, DokumentTagIzvod>,
        IDokumentTagIzvodManager
    {
        public DokumentTagIzvodManager(ILogger<DokumentTagIzvodManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {

        }

        public LSCoreListResponse<DokumentTagIzvodGetDto> GetMultiple(DokumentTagIzvodGetMultipleRequest request)
        {
            var response = new LSCoreListResponse<DokumentTagIzvodGetDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;


            response.Payload = qResponse.Payload!
                .Where(x =>
                    (!request.BrDok.HasValue || x.BrojDokumentaIzvoda == request.BrDok) &&
                    (request.Korisnici == null ||  request.Korisnici.Contains(x.Korisnik)))
                .ToList()
                .ToListDto();

            return response;
        }

        public LSCoreResponse<DokumentTagIzvodGetDto> Save(DokumentTagizvodPutRequest request)
        {
            var response = new LSCoreResponse<DokumentTagIzvodGetDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var entity = base.Save(request);

            var dto = new DokumentTagIzvodGetDto();
            dto.InjectFrom(entity);
            response.Payload = dto;

            return response;
        }
    }
}
