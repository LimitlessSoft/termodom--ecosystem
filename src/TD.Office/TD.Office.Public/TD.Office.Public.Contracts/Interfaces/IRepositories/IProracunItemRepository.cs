using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IProracunItemRepository
{
    ProracunItemEntity? GetOrDefault(long id);
    ProracunItemEntity Get(long id);
    void UpdateKolicina(long id, decimal kolicina);
    void Update(ProracunItemEntity item);
}
