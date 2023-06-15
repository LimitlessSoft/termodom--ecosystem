using FirebirdSql.Data.FirebirdClient;
using Lamar;
using Microsoft.EntityFrameworkCore;
using TD.Core.Framework;
using TD.Core.Framework.Extensions;
using TD.DbMigrations.Contracts.IManagers;
using TD.Webshop.Repository;

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
            //services.AddSingleton<IWebApiRequestManager, WebApiRequestManager>();
            //services.AddSingleton<IMigrationManager, MigrationManager>();
            //services.AddSingleton<IUsersManager, UsersManager>();
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();

            logger.LogInformation("Hello");

            using(FbConnection con = new FbConnection("data source=192.168.0.3; initial catalog = E:\\4monitor\\Poslovanje\\Baze\\TDOffice_v2\\TDOffice_v2_2021.FDB; user=SYSDBA; password=masterkey"))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("SELECT * FROM MAGACIN", con))
                {
                    using(FbDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            var a = dr["NAZIV"].ToString();
                        }
                    }
                }
            }
            //var dbContext = serviceProvider.GetService<Webshop.DataContext.DatabaseContext>();
            //dbContext.Database.Migrate();
        }
    }
}