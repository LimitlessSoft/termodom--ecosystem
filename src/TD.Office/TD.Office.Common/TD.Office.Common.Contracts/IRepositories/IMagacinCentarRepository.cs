using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Contracts.IRepositories;

public interface IMagacinCentarRepository
{
    MagacinCentarEntity? GetOrDefaultByMagaicnId(int magacinId);
    List<MagacinCentarEntity> GetAll();
    List<MagacinCentarEntity> GetAllContainingMagacinIds(List<int> magacinIds);
}
