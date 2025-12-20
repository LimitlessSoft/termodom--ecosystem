using LSCore.ApiClient.Rest;
using LSCore.Auth.Contracts;
using LSCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using TD.Komercijalno.Client;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Common.Repository;
using TD.Office.InterneOtpremnice.Client;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Tests;

public abstract class TestBase
{
	protected static readonly object Lock = new();
	protected readonly OfficeDbContext _dbContext;

	protected TestBase()
	{
		var builder = Host.CreateApplicationBuilder();

		var options = new DbContextOptionsBuilder<OfficeDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		var configurationMock = new Mock<IConfigurationRoot>();
		configurationMock.Setup(c => c["POSTGRES_HOST"]).Returns("localhost");
		configurationMock.Setup(c => c["POSTGRES_PORT"]).Returns("5432");
		configurationMock.Setup(c => c["POSTGRES_USER"]).Returns("user");
		configurationMock.Setup(c => c["POSTGRES_PASSWORD"]).Returns("pass");
		configurationMock.Setup(c => c["DEPLOY_ENV"]).Returns("test");

		var officeDbContext = new TestOfficeDbContext(options, configurationMock.Object);

		builder.Services.AddScoped<OfficeDbContext>(_ => officeDbContext);
		builder.Services.AddScoped<DbContext>(_ => officeDbContext);
		builder.Services.AddLogging();
		builder.Services.AddMemoryCache();

		builder.Services.AddSingleton<IDistributedCache>(new Mock<IDistributedCache>().Object);
		builder.Services.AddSingleton(new Mock<LSCoreAuthContextEntity<string>>().Object);
		builder.Services.AddSingleton(new Mock<IMagacinCentarRepository>().Object);
		builder.Services.AddSingleton(
			new Mock<TDOfficeInterneOtpremniceClient>(
				new LSCoreApiClientRestConfiguration<TDOfficeInterneOtpremniceClient>
				{
					BaseUrl = "http://localhost",
				}
			).Object
		);
		builder.Services.AddSingleton(new Mock<IKomercijalnoMagacinFirmaRepository>().Object);
		builder.Services.AddSingleton(new Mock<IModuleHelpRepository>().Object);
		builder.Services.AddSingleton(new Mock<ILogRepository>().Object);
		builder.Services.AddSingleton(
			new Mock<IKomercijalnoPriceKoeficijentEntityRepository>().Object
		);
		builder.Services.AddSingleton(new Mock<IUserRepository>().Object);

		// Removed builder.AddLSCoreDependencyInjection("TD.Office.Public") because it validates all dependencies
		// and we are manually instantiating what we need in tests for now, or we can add specific registrations.

		builder.Services.AddSingleton(configurationMock.Object);
		builder.Services.AddSingleton(new Mock<ITDKomercijalnoClientFactory>().Object);
		builder.Services.AddSingleton(
			new Mock<TDKomercijalnoClient>(
				2025,
				TDKomercijalnoEnvironment.Development,
				TDKomercijalnoFirma.TCMDZ
			).Object
		);

		var host = builder.Build();
		host.UseLSCoreDependencyInjection();

		_dbContext = officeDbContext;
	}
}
