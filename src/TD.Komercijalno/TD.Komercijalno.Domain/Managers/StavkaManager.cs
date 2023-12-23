using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class StavkaManager : LSCoreBaseManager<StavkaManager, Stavka>, IStavkaManager
    {
        private readonly IProcedureManager _procedureManager;

        public StavkaManager(ILogger<StavkaManager> logger, KomercijalnoDbContext dbContext, IProcedureManager procedureManager)
            : base(logger, dbContext)
        {
            _procedureManager = procedureManager;
        }

        public LSCoreResponse<StavkaDto> Create(StavkaCreateRequest request)
        {
            var response = new LSCoreResponse<StavkaDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var stavka = new Stavka();

            var dokumentResponse = First<Dokument>(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);

            if (dokumentResponse.Status == System.Net.HttpStatusCode.NotFound)
                return LSCoreResponse<StavkaDto>.BadRequest($"Dokument vrDok: {request.VrDok}, brDok {request.BrDok} nije pronadjen");

            response.Merge(dokumentResponse);
            if (response.NotOk)
                return response;

            var qRobaResponse = Queryable<Roba>();
            response.Merge(qRobaResponse);
            if (response.NotOk)
                return response;

            var roba = qRobaResponse.Payload!
                .Include(x => x.Tarifa)
                .Where(x => x.Id == request.RobaId)
                .FirstOrDefault();

            if(roba == null)
                return LSCoreResponse<StavkaDto>.BadRequest($"Roba sa ID-em: {request.RobaId} nije pronadjena u globalnom sifarniku robe.");

            if (string.IsNullOrWhiteSpace(request.Naziv))
                request.Naziv = roba?.Naziv ?? "Undefined";

            var getCenaNaDanResponse = _procedureManager.GetProdajnaCenaNaDan(new Contracts.Requests.Procedure.ProceduraGetProdajnaCenaNaDanRequest() {
                Datum = DateTime.Now,
                MagacinId = dokumentResponse.Payload.MagacinId, RobaId = request.RobaId });
            if (getCenaNaDanResponse.NotOk)
                return LSCoreResponse<StavkaDto>.InternalServerError();

            var prodajnaCenaBezPdvNaDan = getCenaNaDanResponse.Payload / ((100d + roba.Tarifa.Stopa) / 100d);

            if (request.ProdajnaCenaBezPdv == null)
                request.ProdajnaCenaBezPdv = prodajnaCenaBezPdvNaDan;

            if (request.ProdajnaCenaBezPdv != prodajnaCenaBezPdvNaDan)
                request.Rabat = ((request.ProdajnaCenaBezPdv.Value / prodajnaCenaBezPdvNaDan) - 1) * -100;

            if(request.NabavnaCena == null)
            {
                var robaUMagacinuResponse = First<RobaUMagacinu>(x => x.MagacinId == dokumentResponse.Payload.MagacinId && x.RobaId == request.RobaId);
                request.NabavnaCena = robaUMagacinuResponse.Status == System.Net.HttpStatusCode.NotFound ? 0 : robaUMagacinuResponse.Payload.NabavnaCena;
            }

            var magacinResponse = First<Magacin>(x => x.Id == dokumentResponse.Payload.MagacinId);
            response.Merge(magacinResponse);
            if (response.NotOk)
                return response;

            stavka.InjectFrom(request);

            stavka.Vrsta = roba.Vrsta;
            stavka.MagacinId = dokumentResponse.Payload.MagacinId;
            stavka.ProdCenaBp = request.ProdajnaCenaBezPdv ?? 0;
            stavka.ProdajnaCena = getCenaNaDanResponse.Payload;
            stavka.DevProdCena = getCenaNaDanResponse.Payload / dokumentResponse.Payload.Kurs;
            stavka.TarifaId = roba.TarifaId;
            stavka.Porez = roba.Tarifa.Stopa;
            stavka.PorezIzlaz = roba.Tarifa.Stopa;
            stavka.PorezUlaz = roba.Tarifa.Stopa;
            stavka.NabCenSaPor = request.NabCenaSaPor ?? 0;
            stavka.FakturnaCena = request.FakturnaCena ?? 0;
            stavka.NabCenaBt = request.NabCenaBt ?? 0;
            stavka.Troskovi = request.Troskovi ?? 0;
            stavka.Korekcija = request.Korekcija ?? 0;
            stavka.MtId = magacinResponse.Payload.MtId;

            Insert<Stavka>(stavka);

            response.Status = System.Net.HttpStatusCode.Created;
            response.Payload = stavka.ToStavkaDto();
            return response;
        }

        public LSCoreListResponse<StavkaDto> GetMultiple(StavkaGetMultipleRequest request)
        {
            var response = new LSCoreListResponse<StavkaDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var query = qResponse.Payload!
                .Where(x =>
                    (request.VrDok == null || request.VrDok.Length == 0 || request.VrDok.Contains(x.VrDok)) &&
                    (request.MagacinId == null || request.MagacinId.Length == 0 || request.MagacinId.Contains(x.MagacinId)));

            if (request.DokumentFilter != null)
            {
                query
                    .Include(x => x.Dokument)
                    .Where(x => 
                        (request.DokumentFilter.PPID == null || x.Dokument.PPID == request.DokumentFilter.PPID) &&
                        (request.DokumentFilter.DatumOd == null || x.Dokument.Datum >= request.DokumentFilter.DatumOd) &&
                        (request.DokumentFilter.DatumDo == null || x.Dokument.Datum >= request.DokumentFilter.DatumDo) &&
                        (request.DokumentFilter.Flag == null || x.Dokument.Flag == request.DokumentFilter.Flag));
            }
            throw new NotImplementedException();
        }
    }
}
