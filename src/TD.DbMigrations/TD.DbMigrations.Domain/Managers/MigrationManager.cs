using TD.DbMigrations.Contracts.IManagers;
using TD.Webshop.Contracts.IManagers;
using Microsoft.Extensions.Logging;

namespace TD.DbMigrations.Domain.Managers
{
    public class MigrationManager : IMigrationManager
    {
        private readonly ILogger<MigrationManager> _logger;
        private readonly IUsersManager _usersManager;

        public MigrationManager(ILogger<MigrationManager> logger, IUsersManager usersManager)
        {
            _logger = logger;
            _usersManager = usersManager;
        }

        public void StartMigration()
        {
        }
    }
}
