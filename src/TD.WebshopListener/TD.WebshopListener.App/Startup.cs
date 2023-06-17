using FirebirdSql.Data.FirebirdClient;
using Lamar;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Messages;
using TD.Core.Domain.Managers;
using TD.Core.Framework;
using TD.Core.Framework.Extensions;
using TD.DbMigrations.Contracts.IManagers;
using TD.Webshop.Repository;
using TD.WebshopListener.Contracts.Constants;
using TD.WebshopListener.Contracts.Dtos;
using TD.WebshopListener.Contracts.IManagers;
using TD.WebshopListener.Contracts.Messages;
using TD.WebshopListener.Domain.Managers;

namespace TD.WebshopListener.App
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
            services.AddSingleton<IWebshopApiManager, WebshopApiManager>();
            services.AddSingleton<ITDBrainApiManager, TDBrainApiManager>();
            services.AddSingleton<ITaskSchedulerManager, TaskSchedulerManager>();
            services.AddSingleton<IWorkManager, WorkManager>();
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            if (logger == null)
                return;

            logger.LogInformation("Webshop Listener Started");

            var workManager = serviceProvider.GetService<WorkManager>();
            if (workManager == null)
            {
                logger.LogError(Core.Contracts.Messages.CommonMessages.ObjectCannotBeNull(nameof(workManager)));
                return;
            }

            _ = workManager.StartListeningWebshopAkcAsync();
        }
    }
}