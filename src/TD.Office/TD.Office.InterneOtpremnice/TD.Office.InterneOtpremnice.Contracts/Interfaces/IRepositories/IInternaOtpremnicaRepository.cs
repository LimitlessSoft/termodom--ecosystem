using TD.Office.InterneOtpremnice.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;

public interface IInternaOtpremnicaRepository
{
    InternaOtpremnicaEntity? GetOrDefault(long id);
    InternaOtpremnicaEntity Get(long id);
    InternaOtpremnicaEntity Create(int polazniMagacinId, int dolazniMagacinId);
    IQueryable<InternaOtpremnicaEntity> GetMultiple();
}