using Microsoft.AspNetCore.Authentication.JwtBearer;
using Lamar.Microsoft.DependencyInjection;
using LSCore.Framework.Extensions.Lamar;
using LSCore.Contracts.SettingsModels;
using TD.Web.Common.Contracts.Helpers;
using LSCore.Framework.Middlewares;
using TD.Web.Admin.Api.Middlewares;
using LSCore.Framework.Extensions;
using Microsoft.OpenApi.Models;
using TD.Web.Common.Repository;
using LSCore.Domain;
using Lamar;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from json file and environment variables
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register IHttpContextAccessor outside UseLamar to avoid issues with middleware
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// All services registration should go here
builder.Services.AddCors(options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Using lamar as DI container
builder.Host.UseLamar((_, registry) =>
{

    // Register configuration root
    builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);

    // Register services
    registry.Scan(x =>
    {
        x.TheCallingAssembly();
        x.AssembliesAndExecutablesFromApplicationBaseDirectory((a) => a.GetName().Name!.StartsWith("TD.Web"));

        x.WithDefaultConventions();
        x.LSCoreServicesLamarScan();
    });
    
    registry.For<LSCoreMinioSettings>().Use(
        new LSCoreMinioSettings()
        {
            BucketBase = GeneralHelpers.GenerateBucketName(builder.Configuration["DEPLOY_ENV"]!),
            Host = builder.Configuration["MINIO_HOST"]!,
            AccessKey = builder.Configuration["MINIO_ACCESS_KEY"]!,
            SecretKey = builder.Configuration["MINIO_SECRET_KEY"]!,
            Port = builder.Configuration["MINIO_PORT"]!
        });
//             
// services.For<LSCoreApiKeysSettings>().Use(new LSCoreApiKeysSettings()
// {
//     ApiKeys = new List<string>()
//     {
//         "2v738br3t89abtv8079yfc9q324yr7n7qw089rcft3y2w978"
//     }
// });

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
});

// Add dotnet logging
builder.LSCoreAddLogging();

var app = builder.Build();

LSCoreDomainConstants.Container = app.Services.GetService<IContainer>();

app.UseCors("default");

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
app.UseMiddleware<WebAdminAuthorizationMiddleware>();

app.MapControllers();

app.Run();