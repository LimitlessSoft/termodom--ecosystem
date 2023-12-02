using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TD.Web.Common.Repository;
using TD.Web.Common.Contracts;
using LSCore.Contracts.Interfaces;
using LSCore.Repository;
using LSCore.Framework;

namespace TD.Web.Common.DbMigrations
{
    public class Startup : LSCoreBaseStartup, ILSCoreMigratable
    {
        private const string ProjectName = "TD.Web.Common";

        public Startup()
            : base(ProjectName)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            ConfigurationRoot.ConfigureNpgsqlDatabase<WebDbContext, Startup>(services, Constants.DbName);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);
        }
    }
}