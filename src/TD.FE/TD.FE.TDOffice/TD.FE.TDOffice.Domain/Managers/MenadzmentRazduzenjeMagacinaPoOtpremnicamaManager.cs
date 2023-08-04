using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.FE.TDOffice.Contracts.Dtos.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager : BaseManager<MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager>, IMenadzmentRazduzenjeMagacinaPoOtpremnicamaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager(ILogger<MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public Response<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto> PripremaDokumenata(PripremaDokumenataRequest request)
        {
            var response = new Response<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto>();

            if (request.IsRequestInvalid(response))
                return response;

            var komercijalnoDokumentiResponse = _komercijalnoApiManager.GetAsync<DokumentGetMultipleRequest, List<DokumentDto>>("/dokumenti", new DokumentGetMultipleRequest()
            {
                VrDok = request.VrDok,
                DatumOd = request.OdDatuma,
                DatumDo = request.DoDatuma,
                MagacinId = request.MagacinId,
                NUID = request.NacinPlacanja,
            }).GetAwaiter().GetResult();
            if (komercijalnoDokumentiResponse.NotOk || komercijalnoDokumentiResponse.Payload == null)
                return Response<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto>.BadRequest();

            response.Payload = new MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto()
            {
                UkupanBrojDokumenata = komercijalnoDokumentiResponse.Payload.Count,
                ZbirnaVrednostDokumenataSaCenamaNaDanasnjiDan = komercijalnoDokumentiResponse.Payload.Sum(x => x.Duguje)
            };
            return response;
        }

        public Response RazduziMagacin(RazduziMagacinRequest request)
        {
            var response = new Response();

            if (request.IsRequestInvalid(response))
                return response;

            if (request.Izvor.IsRequestInvalid(response))
                return response;

            if(request.NoviDokument)
            {
                response.Status = System.Net.HttpStatusCode.NotImplemented;
                return response;
            }

            if (request.DestinacijaBrDok == null)
                return Response.BadRequest();

            var destinacioniDokument = _komercijalnoApiManager.GetAsync<DokumentDto>($"/dokumenti/{request.DestinacijaVrDok}/{request.DestinacijaBrDok}").GetAwaiter().GetResult();

            if (destinacioniDokument.NotOk || destinacioniDokument.Payload == null)
                return Response.BadRequest();

            var izvorniDokumenti = _komercijalnoApiManager.GetAsync<DokumentGetMultipleRequest, List<DokumentDto>>("/dokumenti", new DokumentGetMultipleRequest()
            {
                VrDok = request.Izvor.VrDok,
                DatumOd = request.Izvor.OdDatuma,
                DatumDo = request.Izvor.DoDatuma,
                MagacinId = request.Izvor.MagacinId,
                NUID = request.Izvor.NacinPlacanja,
            }).GetAwaiter().GetResult();

            if (izvorniDokumenti.NotOk || izvorniDokumenti.Payload == null)
                return Response.BadRequest();

            // validatori za destinacioni dokument

            var stavke = new Dictionary<int, double>();

            izvorniDokumenti.Payload.ForEach(x =>
            {
                x.Stavke?.ForEach(y =>
                {
                    if (stavke.ContainsKey(y.RobaId))
                        stavke[y.RobaId] += y.Kolicina;
                    else
                        stavke.Add(y.RobaId, y.Kolicina);
                });
            });

            foreach(var key in stavke.Keys)
            {
                var createStavka = _komercijalnoApiManager.PostAsync<StavkaCreateRequest, StavkaDto>("/stavke", new StavkaCreateRequest()
                {
                    BrDok = destinacioniDokument.Payload.BrDok,
                    VrDok = destinacioniDokument.Payload.VrDok,
                    RobaId = key,
                    Rabat = 0,
                    Kolicina = stavke[key]
                }).GetAwaiter().GetResult();
                if (createStavka.NotOk)
                    return Response.BadRequest();
            }

            return response;
        }
    }
}
