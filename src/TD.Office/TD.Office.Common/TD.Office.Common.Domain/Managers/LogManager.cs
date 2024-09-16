using LSCore.Contracts;
using LSCore.Contracts.IManagers;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Repository;

namespace TD.Office.Common.Domain.Managers;

public class LogManager(
    ILogger<LogManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser
) : LSCoreManagerBase<LogManager, LogEntity>(logger, dbContext, currentUser), ILogManager
{
    public void Log(LogKey key)
    {
        var log = new LogEntity { Key = key };
        Insert(log);
    }

    public void Log(LogKey key, string value)
    {
        var log = new LogEntity() { Key = key, Value = value };
        Insert(log);
    }
}
