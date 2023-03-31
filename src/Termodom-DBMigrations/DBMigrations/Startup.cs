using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DBMigrations
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; private set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => Configuration);
            services.AddSingleton<OldApi>();
            services.AddControllers();
            services.AddDbContext<DbMigrationsDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseNpgsql("Server=192.168.0.3;Port=65432;Userid=postgres;Password=Plivanje123$;Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database=termodom-dev");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
#if DEBUG
            app.UseDeveloperExceptionPage();
#endif
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}