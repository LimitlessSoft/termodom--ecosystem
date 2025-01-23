using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface INalogZaPrevozRepository
{
    NalogZaPrevozEntity Get(long id);
    NalogZaPrevozEntity? GetOrDefault(long id);
    void UpdateOrCreate(NalogZaPrevozEntity entity);
    IQueryable<NalogZaPrevozEntity> GetMultiple();
}
