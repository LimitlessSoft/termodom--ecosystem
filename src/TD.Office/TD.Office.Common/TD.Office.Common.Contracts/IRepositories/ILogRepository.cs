using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.IRepositories;

public interface ILogRepository
{
    void Create(LogKey key);
    void Create(LogKey key, string value);
    IQueryable<LogEntity> GetMultiple();
}
