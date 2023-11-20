using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class ProcedureManager : LSCoreBaseManager<ProcedureManager>, IProcedureManager
    {
        public ProcedureManager(ILogger<ProcedureManager> logger, KomercijalnoDbContext dbContext) : base(logger, dbContext)
        {

        }

        public LSCoreResponse<double> GetProdajnaCenaNaDan(ProceduraGetProdajnaCenaNaDanRequest request)
        {
            var response = new LSCoreResponse<double>();
            if (request.IsRequestInvalid(response))
                return response;

            var poslednjaStavka = Queryable<Stavka>()
                .Include(x => x.Dokument)
                .ThenInclude(x => x.VrstaDok)
                .Include(x => x.Magacin)
                .Where(x =>
                    x.Dokument.Datum <= request.Datum &&
                    (x.Dokument.Linked != null && x.Dokument.Linked != "9999999999") &&
                    x.Dokument.KodDok == 0 &&
                    x.RobaId == request.RobaId &&
                    (request.ZaobidjiBrDok == null ||
                        x.Dokument.VrDok != request.ZaobidjiVrDok &&
                        x.Dokument.BrDok != request.ZaobidjiBrDok) &&

                    x.MagacinId == request.MagacinId &&
                    x.Dokument.VrstaDok.DefiniseCenu == 1 &&
                    x.Dokument.VrstaDok.ImaKarticu.HasValue &&
                    x.Dokument.VrstaDok.ImaKarticu == 1)
                .OrderByDescending(x => x.Dokument.Datum)
                .ThenByDescending(x => x.Dokument.Linked)
                .ThenByDescending(x => x.Dokument.VrstaDok.Io)
                .ThenByDescending(x => x.VrDok)
                .ThenByDescending(x => x.BrDok)
                .ThenByDescending(x => x.Id)
                .FirstOrDefault();

            if (poslednjaStavka == null)
                return new LSCoreResponse<double>(0);

            return new LSCoreResponse<double>(poslednjaStavka.Magacin.VodiSe == 4 ? poslednjaStavka.NabavnaCena : poslednjaStavka.ProdajnaCena);
        }
    }
}
