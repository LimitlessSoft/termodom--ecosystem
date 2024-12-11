using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers;

public class WebDbContextFactory(IConfigurationRoot configurationRoot) : IWebDbContextFactory
{
    /*
     *AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
       serviceCollection
           .AddEntityFrameworkNpgsql()
           .AddDbContext<WebDbContext>(
               (serviceProvider, options) =>
               {
                   options.ConfigureDbContext(configurationRoot, "TD.Web.Common.DbMigrations");
               }
           );
     *
     */
    public T Create<T>()
    {
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<WebDbContext>();
        dbContextOptionsBuilder.ConfigureDbContext(configurationRoot, "TD.Web.Common.DbMigrations");
        return (T)Activator.CreateInstance(typeof(T), dbContextOptionsBuilder.Options);
    }
}
