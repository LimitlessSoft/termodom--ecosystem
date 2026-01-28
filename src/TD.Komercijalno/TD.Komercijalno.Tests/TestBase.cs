using LSCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Tests;

public abstract class TestBase
{
	protected static readonly object Lock = new();
	protected readonly KomercijalnoDbContext _dbContext;

	protected TestBase()
	{
		var builder = Host.CreateApplicationBuilder();

		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		builder.Services.AddScoped<KomercijalnoDbContext>(_ => new KomercijalnoDbContext(options));
		builder.Services.AddScoped<DbContext>(_ => new KomercijalnoDbContext(options));
		builder.Services.AddLogging();

		IHost host;
		// Lock to prevent parallel test execution from causing collection modification errors
		// in LSCore's static assembly scanning during initialization
		lock (Lock)
		{
			builder.AddLSCoreDependencyInjection("TD.Komercijalno");
			host = builder.Build();
			host.UseLSCoreDependencyInjection();
		}

		_dbContext = new KomercijalnoDbContext(options);
	}
}
