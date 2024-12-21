using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository.Repositories;

public class MagacinCentarRepository(OfficeDbContext dbContext) : IMagacinCentarRepository
{
    public MagacinCentarEntity? GetOrDefaultByMagaicnId(int magacinId) =>
        dbContext.MagacinCentri.FirstOrDefault(x => x.IsActive && x.MagacinIds.Contains(magacinId));

    public List<MagacinCentarEntity> GetAll() =>
        dbContext.MagacinCentri.Where(x => x.IsActive).ToList();

    public List<MagacinCentarEntity> GetAllContainingMagacinIds(List<int> magacinIds) =>
        dbContext
            .MagacinCentri.Where(x => x.IsActive && x.MagacinIds.Any(id => magacinIds.Contains(id)))
            .ToList();
}
