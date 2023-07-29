using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.MenadzmentRazduzenjeMagacinaPoOtpremnicama;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IMenadzmentRazduzenjeMagacinaPoOtpremnicamaManager
    {
        Response<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto> PripremaDokumenata();
    }
}
