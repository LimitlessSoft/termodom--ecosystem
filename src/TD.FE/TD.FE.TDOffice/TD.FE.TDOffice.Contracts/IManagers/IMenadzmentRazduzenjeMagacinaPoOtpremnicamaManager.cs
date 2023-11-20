using LSCore.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.MenadzmentRazduzenjeMagacinaPoOtpremnicama;
using TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IMenadzmentRazduzenjeMagacinaPoOtpremnicamaManager
    {
        LSCoreResponse<MenadzmentRazduzenjeMagacinaPoOtpremnicamaPripremaDokumenataDto> PripremaDokumenata(PripremaDokumenataRequest request);
        LSCoreResponse RazduziMagacin(RazduziMagacinRequest request);
    }
}
