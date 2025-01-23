using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Domain.Managers;

public class LogManager(ILogRepository logRepository) : ILogManager
{
    public void Log(LogKey key) => logRepository.Create(key);

    public void Log(LogKey key, string value) => logRepository.Create(key, value);
}
