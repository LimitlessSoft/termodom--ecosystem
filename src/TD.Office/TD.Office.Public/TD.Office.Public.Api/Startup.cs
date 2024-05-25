using Lamar;
using LSCore.Contracts;
using TD.Office.Common.Repository;
using TD.Office.Common.Contracts;
using LSCore.Framework;
using LSCore.Contracts.Interfaces;
using LSCore.Repository;
using Microsoft.IdentityModel.Tokens;

namespace TD.Office.Public.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        
        public Startup()
            : base(Constants.ProjectName, true, false)
        {
            AfterAuthenticationMiddleware = (builder) =>
            {
                builder.Use(async (context, next) =>
                {
                    var endpoint = context.GetEndpoint();
                    
                    var authorizationAttribute = endpoint?.Metadata.GetMetadata<LSCoreAuthorizationAttribute>();
                    if (authorizationAttribute != null)
                    {
                        var userId = context.User.FindFirst(LSCoreContractsConstants.ClaimNames.CustomUserId)?.Value;
                        var dbContext = context.RequestServices.GetService<OfficeDbContext>();
                        var user = dbContext!.Users.Find(Convert.ToInt32(userId));
                        if (user == null)
                        {
                            context.Response.StatusCode = 403;
                            return;
                        }

                        if (authorizationAttribute.Roles.IsNullOrEmpty())
                        {
                            await next();
                            return;
                        }
                        
                        if (!authorizationAttribute.Roles!.Contains(user.Type.ToString()))
                        {
                            context.Response.StatusCode = 403;
                            return;
                        }
                    }
                    
                    await next();
                });
                return builder;
            };
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            ConfigurationRoot.ConfigureNpgsqlDatabase<OfficeDbContext, Startup>(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            applicationBuilder.UseCors("default");

            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger.LogInformation("Application started!");
        }
    }
}