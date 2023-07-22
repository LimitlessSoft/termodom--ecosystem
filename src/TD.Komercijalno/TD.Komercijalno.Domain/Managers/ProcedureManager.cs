using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class ProcedureManager : BaseManager<ProcedureManager>, IProcedureManager
    {
        public ProcedureManager(ILogger<ProcedureManager> logger, KomercijalnoDbContext dbContext) : base(logger, dbContext)
        {

        }

        public Response<double> GetProdajnaCenaNaDan(ProceduraGetProdajnaCenaNaDanRequest request)
        {
            var poslednjaStavka = Queryable<Stavka>()
                .Include(x => x.Dokument)
                .ThenInclude(x => x.VrstaDok)
                .Include(x => x.Magacin)
                .Where(x =>
                    x.Dokument.Datum <= request.Datum &&
                    (x.Dokument.Linked != null && x.Dokument.Linked != "9999999999") &&
                    x.Dokument.KodDok == 0 &&
                    x.RobaId == request.RobaId &&
                    x.MagacinId == request.MagacinId &&
                    x.Dokument.VrstaDok.ImaKarticu.HasValue &&
                    x.Dokument.VrstaDok.ImaKarticu == 1 &&
                    x.Kolicina >= 0) // TODO: Da li stvarno da uzima sa kolicinom vecom od 0 samo?
                .OrderByDescending(x => x.Dokument.Datum)
                .ThenByDescending(x => x.Dokument.Linked)
                .ThenByDescending(x => x.Dokument.VrstaDok.Io)
                .ThenByDescending(x => x.VrDok)
                .ThenByDescending(x => x.BrDok)
                .ThenByDescending(x => x.StavkaId)
                .FirstOrDefault();

            if (poslednjaStavka == null)
                return new Response<double>(0);

            return new Response<double>(poslednjaStavka.Magacin.VodiSe == 4 ? poslednjaStavka.NabavnaCena : poslednjaStavka.ProdajnaCena);
        }
    }
}
