using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.TDOffice.Contracts.DtoMappings;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers
{
    public class DokumentTagIzvodManager : BaseManager<DokumentTagIzvodManager, DokumentTagIzvod>,
        IDokumentTagIzvodManager
    {
        public DokumentTagIzvodManager(ILogger<DokumentTagIzvodManager> logger, TDOfficeDbContext dbContext)
            : base(logger, dbContext)
        {

        }

        public ListResponse<DokumentTagIzvodGetDto> GetMultiple(DokumentTagIzvodGetMultipleRequest request)
        {
            var response = new ListResponse<DokumentTagIzvodGetDto>();
            response.Payload = Queryable()
                .Where(x =>
                    (!request.BrDok.HasValue || x.BrojDokumentaIzvoda == request.BrDok) &&
                    (request.Korisnici == null ||  request.Korisnici.Contains(x.Korisnik)))
                .ToList()
                .ToListDto();

            return response;
        }

        public Response<DokumentTagIzvodGetDto> Save(DokumentTagizvodPutRequest request)
        {
            var response = new Response<DokumentTagIzvodGetDto>();

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
