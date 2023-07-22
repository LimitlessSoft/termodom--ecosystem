using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.TDOffice.Contracts.DtoMappings;
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

        public ListResponse<DokumentTagIzvod> GetMultiple(DokumentTagIzvodGetMultipleRequest request)
        {
            var response = new ListResponse<DokumentTagIzvod>();
            response.Payload = Queryable()
                .Where(x =>
                    (!request.BrDok.HasValue || x.BrojDokumentaIzvoda == request.BrDok) &&
                    (request.Korisnici == null ||  request.Korisnici.Contains(x.Korisnik)))
                .ToList();
            return response;
        }

        public Response<bool> Save(DokumentTagizvodPutRequest request)
        {
            var response = new Response<bool>();

            if (request.IsRequestInvalid(response))
                return response;

            base.Save(request, (x, y) => x.ToDokumentTagIzvod());
            return new Response<bool>(true);
        }
    }
}
