using LSCore.Contracts.Requests;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;

public interface IInterneOtpremniceManager
{
    InternaOtpremnicaDto Get(LSCoreIdRequest request);
    InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request);
    List<InternaOtpremnicaDto> GetMultiple();
}
