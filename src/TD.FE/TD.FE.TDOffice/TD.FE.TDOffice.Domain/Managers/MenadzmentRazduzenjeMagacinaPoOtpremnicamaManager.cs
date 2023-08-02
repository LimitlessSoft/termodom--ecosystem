using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.FE.TDOffice.Contracts.Dtos.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;

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
                ZbirnaVrednostDokumenataSaCenamaNaDanasnjiDan = komercijalnoDokumentiResponse.Payload.Sum(x => x.Potrazuje)
            };
            return response;
        }
    }
}
