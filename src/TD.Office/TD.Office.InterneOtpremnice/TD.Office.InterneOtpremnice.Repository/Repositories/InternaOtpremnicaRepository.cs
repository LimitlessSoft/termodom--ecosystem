using LSCore.Contracts.Exceptions;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;

namespace TD.Office.InterneOtpremnice.Repository.Repositories;

public class InternaOtpremnicaRepository(InterneOtpremniceDbContext dbContext)
    : IInternaOtpremnicaRepository
{
    public InternaOtpremnicaEntity? GetOrDefault(long id) =>
        dbContext.InterneOtpremnice.Find(id);

    public InternaOtpremnicaEntity Get(long id)
    {
        var entity = GetOrDefault(id);

        if (entity is null)
            throw new LSCoreNotFoundException();
        
        return entity;
    }

    public InternaOtpremnicaEntity Create(int polazniMagacinId, int dolazniMagacinId)
    {
        var entity = new InternaOtpremnicaEntity
        {
            PolazniMagacinId = polazniMagacinId,
            DestinacioniMagacinId = dolazniMagacinId,
            Status = InternaOtpremnicaStatus.Otkljucana
        };

        dbContext.InterneOtpremnice.Add(entity);
        dbContext.SaveChanges();

        return entity;
    }

    public IQueryable<InternaOtpremnicaEntity> GetMultiple() =>
        dbContext.InterneOtpremnice.AsQueryable();
}