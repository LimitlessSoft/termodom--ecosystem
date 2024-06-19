using Microsoft.AspNetCore.Authentication.JwtBearer;
using Lamar.Microsoft.DependencyInjection;
using LSCore.Framework.Extensions.Lamar;
using TD.Office.Common.Contracts.Models;
using Microsoft.IdentityModel.Tokens;
using LSCore.Framework.Middlewares;
using TD.Office.Common.Repository;
using LSCore.Framework.Extensions;
using TD.Office.Common.Contracts;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using LSCore.Contracts;
using LSCore.Domain;
using System.Text;
using Lamar;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from json file and environment variables
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Using lamar as DI container
builder.Host.UseLamar((_, registry) =>
{
    // All services registration should go here
    
    // Register configuration root
    builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
    
    // Register services
    registry.Scan(x =>
    {
        x.TheCallingAssembly();
        x.AssembliesAndExecutablesFromApplicationBaseDirectory((a) => a.GetName()!.Name!.StartsWith("TD.Office"));
        
        x.WithDefaultConventions();
        x.LSCoreServicesLamarScan();
    });
    
    // Register database
    registry.RegisterDatabase(builder.Configuration);

    registry.AddControllers();

    registry.AddEndpointsApiExplorer();
    registry.AddSwaggerGen(options =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme()
        {
            Name = "JWT Authentication",
            Description = "Put only JWT in this field without Bearer prefix",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Reference = new OpenApiReference()
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };
                
        options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                
        options.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });
    });
    
    registry.AddScoped<CurrentUser>();
    var jwtConfiguration = new JwtConfiguration()
    {
        Key = builder.Configuration[Constants.Jwt.ConfigurationKey]!,
        Issuer = builder.Configuration[Constants.Jwt.ConfigurationIssuer]!,
        Audience = builder.Configuration[Constants.Jwt.ConfigurationAudience]!,
    };

    registry.For<JwtConfiguration>().Use(jwtConfiguration);
    
    registry.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = jwtConfiguration.Issuer,
                ValidateIssuer = true,
                ValidAudience = jwtConfiguration.Audience,
                ValidateActor = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key)),
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true
            };
        });
});

// Add dotnet logging
builder.LSCoreAddLogging();

var app = builder.Build();

LSCoreDomainConstants.Container = app.Services.GetService<IContainer>();

// Add exception handling middleware
// It is used to handle exceptions globally
app.UseMiddleware<LSCoreHandleExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
        
app.Use(async (context, next) =>
{
    var currentUser = context.RequestServices.GetService<CurrentUser>();

    if (context.User.Identity?.IsAuthenticated == true)
    {
        currentUser!.Id = long.Parse(context.User.FindFirstValue(LSCoreContractsConstants.ClaimNames.CustomUserId)!);
    }

    await next();
});

app.MapControllers();

app.Run();