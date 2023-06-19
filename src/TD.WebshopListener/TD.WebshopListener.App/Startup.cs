using Lamar;
using Microsoft.EntityFrameworkCore;
using TD.Core.Framework;
using TD.Webshop.Repository;
using TD.WebshopListener.Contracts.IManagers;

namespace TD.WebshopListener.App
{
    public class Startup : BaseStartup
    {
        public Startup() : base("TD.WebshopListener")
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddTransient<DbContext, WebshopDbContext>();
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            if (logger == null)
                return;

            logger.LogInformation("Webshop Listener Started");

            var workManager = serviceProvider.GetService<IWorkManager>();
            if (workManager == null)
            {
                logger.LogError(Core.Contracts.Messages.CommonMessages.ObjectCannotBeNull(nameof(workManager)));
                return;
            }

            _ = workManager.StartListeningWebshopAkcAsync();
        }
    }
}