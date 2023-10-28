using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TD.Core.Contracts.Interfaces;
using TD.Core.Framework;
using TD.Web.Common.Repository;
using TD.Core.Repository;

namespace TD.Web.Common.DbMigrations
{
    public class Startup : BaseStartup, IMigratable
    {
        private const string ProjectName = "TD.Web.Common";

        public Startup()
            : base(ProjectName)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            ConfigurationRoot.ConfigureNpgsqlDatabase<WebDbContext, Startup>(services);
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