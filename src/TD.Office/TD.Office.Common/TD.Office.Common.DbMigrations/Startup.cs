using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TD.Core.Contracts.Interfaces;
using TD.Core.Framework;
using TD.Office.Common.Repository;
using TD.Core.Repository;
using TD.Office.Common.Contracts;

namespace TD.Office.Common.DbMigrations
{
    public class Startup : BaseStartup, IMigratable
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