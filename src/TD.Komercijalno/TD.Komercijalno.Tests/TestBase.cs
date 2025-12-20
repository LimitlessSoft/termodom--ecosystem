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

		lock (Lock)
		{
			builder.AddLSCoreDependencyInjection("TD.Komercijalno");
		}

		var host = builder.Build();
		host.UseLSCoreDependencyInjection();

		_dbContext = new KomercijalnoDbContext(options);
	}
}
