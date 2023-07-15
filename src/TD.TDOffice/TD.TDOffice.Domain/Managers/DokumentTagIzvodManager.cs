using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
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
            // ToDo validator da ne moze da se azurira broj dokumenta na postojecem izvodu!
            if (!request.Id.HasValue && request.BrojDokumentaIzvoda.HasValue)
                return Response<bool>.BadRequest("Ne mozete promeniti broj dokumenta izvoda na postojecem itemu!"); // todo prebaciti u validation codes

            Save(request.ToDokumentTagIzvod());
            return new Response<bool>(true);
        }
    }
}
