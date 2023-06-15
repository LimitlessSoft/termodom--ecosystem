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
            //var response = _webApiRequestManager.GetAsync<List<ApiKorisnikListItemDto>>("/webshop/korisnik/list").GetAwaiter().GetResult();
            //if(response.NotOk)
            //{
            //    _logger.Log(LogLevel.Error, $"Error occured in {nameof(MigrationManager.StartMigration)}");
            //    return;
            //}

            //_usersManager.Save();

            //foreach(var user in response.Payload)
            //{

            //}
        }
    }
}
