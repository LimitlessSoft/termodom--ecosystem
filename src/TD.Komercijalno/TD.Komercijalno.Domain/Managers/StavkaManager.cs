using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Helpers;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Domain.Managers
{
    public class StavkaManager : BaseManager<StavkaManager, Stavka>, IStavkaManager
    {
        private readonly IProcedureManager _procedureManager;

        public StavkaManager(ILogger<StavkaManager> logger, KomercijalnoDbContext dbContext, IProcedureManager procedureManager)
            : base(logger, dbContext)
        {
            _procedureManager = procedureManager;
        }

        public Response<StavkaDto> Create(StavkaCreateRequest request)
        {
            var response = new Response<StavkaDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var stavka = new Stavka();

            var dokument = FirstOrDefault<Dokument>(x => x.VrDok == request.VrDok && x.BrDok == request.BrDok);

            if (dokument == null)
                return Response<StavkaDto>.BadRequest($"Dokument vrDok: {request.VrDok}, brDok {request.BrDok} nije pronadjen");

            var roba = Queryable<Roba>()
                .Include(x => x.Tarifa)
                .Where(x => x.Id == request.RobaId)
                .FirstOrDefault();

            if(roba == null)
                return Response<StavkaDto>.BadRequest($"Roba sa ID-em: {request.RobaId} nije pronadjena u globalnom sifarniku robe.");

            if (string.IsNullOrWhiteSpace(request.Naziv))
                request.Naziv = roba?.Naziv ?? "Undefined";

            var getCenaNaDanResponse = _procedureManager.GetProdajnaCenaNaDan(new Contracts.Requests.Procedure.ProceduraGetProdajnaCenaNaDanRequest() { Datum = DateTime.Now, MagacinId = dokument.MagacinId, RobaId = request.RobaId });
            if (getCenaNaDanResponse.NotOk)
                return Response<StavkaDto>.InternalServerError();

            var prodajnaCenaBezPdvNaDan = getCenaNaDanResponse.Payload / ((100d + roba.Tarifa.Stopa) / 100d);

            if (request.ProdajnaCenaBezPdv == null)
                request.ProdajnaCenaBezPdv = prodajnaCenaBezPdvNaDan;

            if (request.ProdajnaCenaBezPdv != prodajnaCenaBezPdvNaDan)
                request.Rabat = ((request.ProdajnaCenaBezPdv.Value / prodajnaCenaBezPdvNaDan) - 1) * -100;

            if(request.NabavnaCena == null)
            {
                var robaUMagacinu = FirstOrDefault<RobaUMagacinu>(x => x.MagacinId == dokument.MagacinId && x.RobaId == request.RobaId);
                request.NabavnaCena = robaUMagacinu?.NabavnaCena ?? 0;
            }

            stavka.InjectFrom(request);

            stavka.Vrsta = roba.Vrsta;
            stavka.MagacinId = dokument.MagacinId;
            stavka.ProdCenaBp = request.ProdajnaCenaBezPdv ?? 0;
            stavka.ProdajnaCena = getCenaNaDanResponse.Payload;
            stavka.TarifaId = roba.TarifaId;
            stavka.Porez = roba.Tarifa.Stopa;
            stavka.PorezIzlaz = roba.Tarifa.Stopa;
            stavka.PorezUlaz = roba.Tarifa.Stopa;
            stavka.MtId = First<Magacin>(x => x.Id == dokument.MagacinId).MtId;

            Insert<Stavka>(stavka);

            response.Status = System.Net.HttpStatusCode.Created;
            response.Payload = stavka.ToStavkaDto();
            return response;
        }

        public ListResponse<StavkaDto> GetMultiple(StavkaGetMultipleRequest request)
        {
            var response = Queryable()
                .Where(x =>
                    (request.VrDok == null || request.VrDok.Length == 0 || request.VrDok.Contains(x.VrDok)) &&
                    (request.MagacinId == null || request.MagacinId.Length == 0 || request.MagacinId.Contains(x.MagacinId)));

            if (request.DokumentFilter != null)
            {
                response
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
