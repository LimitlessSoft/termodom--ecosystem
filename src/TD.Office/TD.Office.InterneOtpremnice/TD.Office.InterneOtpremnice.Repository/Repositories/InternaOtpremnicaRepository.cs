using LSCore.Contracts.Exceptions;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IRepositories;

namespace TD.Office.InterneOtpremnice.Repository.Repositories;

public class InternaOtpremnicaRepository(InterneOtpremniceDbContext dbContext)
    : LSCoreRepositoryBase<InternaOtpremnicaEntity>(dbContext), IInternaOtpremnicaRepository
{
    public InternaOtpremnicaEntity GetDetailed(long internaOtpremnicaId)
    {
        var entity = GetMultiple()
            .Include(z => z.Items)
            .FirstOrDefault(x => x.Id == internaOtpremnicaId);
        if(entity is null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public InternaOtpremnicaEntity Create(int polazniMagacinId, int dolazniMagacinId, long createdBy)
    {
        var entity = new InternaOtpremnicaEntity
        {
            PolazniMagacinId = polazniMagacinId,
            DestinacioniMagacinId = dolazniMagacinId,
            Status = InternaOtpremnicaStatus.Otkljucana,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = createdBy
        };
        dbContext.InterneOtpremnice.Add(entity);
        dbContext.SaveChanges();

        return entity;
    }

    public InternaOtpremnicaItemEntity SaveItem(long? requestId, long internaOtpremnicaId, int robaId,
        decimal kolicina)
    {
        var internaOtpremnica = Get(internaOtpremnicaId);
        var item = requestId.HasValue
            ? dbContext.InterneOtpremniceItems.FirstOrDefault(x => x.Id == requestId.Value && x.IsActive)
            : new InternaOtpremnicaItemEntity
            {
                InternaOtpremnica = internaOtpremnica,
                RobaId = robaId,
                Kolicina = kolicina,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

        if(requestId.HasValue)
        {
            if(item is null)
                throw new LSCoreNotFoundException();
            item.Kolicina = kolicina;
            item.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            dbContext.InterneOtpremniceItems.Add(item!);
        }
        dbContext.SaveChanges();
        return item!;
    }

    public void HardDeleteItem(long requestId)
    {
        var item = dbContext.InterneOtpremniceItems.FirstOrDefault(x => x.Id == requestId && x.IsActive);
        if(item is null)
            throw new LSCoreNotFoundException();
        dbContext.InterneOtpremniceItems.Remove(item);
        dbContext.SaveChanges();
    }

    public void SetStatus(long requestId, InternaOtpremnicaStatus state)
    {
        var entity = Get(requestId);
        entity.Status = state;
        dbContext.SaveChanges();
    }
}