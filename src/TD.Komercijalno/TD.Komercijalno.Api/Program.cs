using Microsoft.AspNetCore.Authentication.JwtBearer;
using Lamar.Microsoft.DependencyInjection;
using LSCore.Framework.Extensions.Lamar;
using LSCore.Framework.Middlewares;
using LSCore.Framework.Extensions;
using TD.Komercijalno.Repository;
using Microsoft.OpenApi.Models;
using LSCore.Domain;
using Lamar;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from json file and environment variables
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
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
        x.AssembliesAndExecutablesFromApplicationBaseDirectory((a) => a.GetName()!.Name!.StartsWith("TD.Komercijalno"));
        
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

app.MapControllers();

app.Run();