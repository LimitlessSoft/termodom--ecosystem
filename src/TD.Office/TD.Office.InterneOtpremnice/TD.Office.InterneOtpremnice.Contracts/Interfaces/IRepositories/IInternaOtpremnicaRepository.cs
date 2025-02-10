using LSCore.Contracts.Interfaces.Repositories;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Enums;

namespace TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;

public interface IInternaOtpremnicaRepository
    : ILSCoreRepositoryBase<InternaOtpremnicaEntity>
{
    InternaOtpremnicaEntity GetDetailed(long internaOtpremnicaId);
    InternaOtpremnicaEntity Create(int polazniMagacinId, int dolazniMagacinId, long createdBy);
    InternaOtpremnicaItemEntity SaveItem(long? requestId, long internaOtpremnicaId, int robaId, decimal kolicina);
    void HardDeleteItem(long requestId);
    void SetStatus(long requestId, InternaOtpremnicaStatus state);
}