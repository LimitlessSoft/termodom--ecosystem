using Lamar;
using Microsoft.EntityFrameworkCore;
using TD.Core.Framework;
using TD.DbMigrations.Contracts.IManagers;
using TD.Webshop.Contracts.IManagers;
using TD.Webshop.Domain.Managers;
using TD.Webshop.Repository;
using TD.WebshopListener.Contracts.IManagers;
using static TD.Core.Framework.Extensions.StartupExtensions;

namespace TD.DbMigrations.App
{
    public class Startup : BaseStartup
    {
        public Startup() : base()
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            ConfigurationRoot.ConfigureDatabase<WebshopDbContext>(services);
            services.AddTransient<DbContext, WebshopDbContext>();
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
            //services.AddSingleton<IWebApiRequestManager, WebApiRequestManager>();
            //services.AddSingleton<IMigrationManager, MigrationManager>();
            //services.AddSingleton<IUsersManager, UsersManager>();
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            //var dbContext = serviceProvider.GetService<Webshop.DataContext.DatabaseContext>();
            //dbContext.Database.Migrate();

            var migrationManager = serviceProvider.GetService<IMigrationManager>();
            migrationManager.StartMigration();
        }
    }
}