using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class MenadzmentRazduzenjeMagacinaPoOtpremnicamaController : Controller
    {
        private readonly IMenadzmentRazduzenjeMagacinaPoOtpremnicamaManager _menadzmentRazduzenjeMagacinaPoOtpremnicamaManager;
        public MenadzmentRazduzenjeMagacinaPoOtpremnicamaController(IMenadzmentRazduzenjeMagacinaPoOtpremnicamaManager menadzmentRazduzenjeMagacinaPoOtpremnicamaManager)
        {
            _menadzmentRazduzenjeMagacinaPoOtpremnicamaManager = menadzmentRazduzenjeMagacinaPoOtpremnicamaManager;
        }

        [HttpGet]
        [Route("/menadzment-razduzenje-magacina-po-otpremnicama/priprema-dokumenata")]
        public Response<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto> PripremaDokumenata()
        {
            return _menadzmentRazduzenjeMagacinaPoOtpremnicamaManager.PripremaDokumenata();
        }
    }
}
