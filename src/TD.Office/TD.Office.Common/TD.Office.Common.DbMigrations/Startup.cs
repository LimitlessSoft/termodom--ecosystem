using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TD.Office.Common.Repository;
using TD.Office.Common.Contracts;
using LSCore.Contracts.Interfaces;
using LSCore.Repository;
using LSCore.Framework;

namespace TD.Office.Common.DbMigrations
{
    public class Startup : LSCoreBaseStartup, ILSCoreMigratable
    {
        private const string ProjectName = "TD.Office.Common";

        public Startup()
            : base(ProjectName)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            ConfigurationRoot.ConfigureNpgsqlDatabase<OfficeDbContext, Startup>(services, Constants.DbName);
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