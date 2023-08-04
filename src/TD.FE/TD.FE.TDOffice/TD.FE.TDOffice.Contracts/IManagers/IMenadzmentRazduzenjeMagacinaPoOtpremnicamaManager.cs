using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IMenadzmentRazduzenjeMagacinaPoOtpremnicamaManager
    {
        Response<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto> PripremaDokumenata(PripremaDokumenataRequest request);
        Response RazduziMagacin(RazduziMagacinRequest request);
    }
}
