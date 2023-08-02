using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.Dtos.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager : BaseManager<MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager>, IMenadzmentRazduzenjeMagacinaPoOtpremnicamaManager
    {
        public MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager(ILogger<MenadzmentRazduzenjeMagacinaPoOtpremnicamaManager> logger)
            : base(logger)
        {
        }

        public Response<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto> PripremaDokumenata()
        {

            throw new NotImplementedException();
        }
    }
}
