using LSCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Tests;

public abstract class TestBase
{
	protected static readonly object Lock = new();
	protected readonly WebDbContext _dbContext;

	protected TestBase()
	{
		var builder = Host.CreateApplicationBuilder();

		var options = new DbContextOptionsBuilder<WebDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		var configurationMock = new Mock<IConfigurationRoot>();
		configurationMock.Setup(c => c["POSTGRES_HOST"]).Returns("localhost");
		configurationMock.Setup(c => c["POSTGRES_PORT"]).Returns("5432");
		configurationMock.Setup(c => c["POSTGRES_USER"]).Returns("user");
		configurationMock.Setup(c => c["POSTGRES_PASSWORD"]).Returns("pass");
		configurationMock.Setup(c => c["DEPLOY_ENV"]).Returns("test");

		var webDbContext = new TestWebDbContext(options, configurationMock.Object);

		builder.Services.AddScoped<WebDbContext>(_ => webDbContext);
		builder.Services.AddScoped<DbContext>(_ => webDbContext);
		builder.Services.AddLogging();

		builder.Services.AddMemoryCache();

		// Register missing common dependencies
		var dbContextFactoryMock = new Mock<IWebDbContextFactory>();
		dbContextFactoryMock.Setup(f => f.Create<WebDbContext>()).Returns(webDbContext);

		var factoryDescriptors = builder
			.Services.Where(d => d.ServiceType == typeof(IWebDbContextFactory))
			.ToList();
		foreach (var descriptor in factoryDescriptors)
		{
			builder.Services.Remove(descriptor);
		}
		builder.Services.AddSingleton(dbContextFactoryMock.Object);

		builder.Services.AddSingleton(new Mock<ICalculatorItemRepository>().Object);
		builder.Services.AddSingleton(new Mock<IOrderItemManager>().Object);
		builder.Services.AddSingleton(new Mock<IPaymentTypeRepository>().Object);
		builder.Services.AddSingleton(new Mock<IProductGroupRepository>().Object);
		builder.Services.AddSingleton(new Mock<IProductRepository>().Object);
		builder.Services.AddSingleton(new Mock<IProductPriceGroupLevelRepository>().Object);
		builder.Services.AddSingleton(new Mock<IStatisticsItemRepository>().Object);
		builder.Services.AddSingleton(new Mock<IOrderItemRepository>().Object);
		builder.Services.AddSingleton(new Mock<IOrderRepository>().Object);
		builder.Services.AddSingleton(new Mock<IUserRepository>().Object);
		builder.Services.AddSingleton(new Mock<ISettingRepository>().Object);
		builder.Services.AddSingleton(new Mock<IStoreRepository>().Object);
		builder.Services.AddSingleton(new Mock<IOfficeServerApiManager>().Object);
		builder.Services.AddSingleton(new Mock<ICacheManager>().Object);
		builder.Services.AddSingleton(new Mock<IImageManager>().Object);
		builder.Services.AddSingleton(new Mock<IOrderManager>().Object);
		builder.Services.AddSingleton(
			new Mock<LSCore.Auth.Contracts.LSCoreAuthContextEntity<string>>().Object
		);

		IHost host;
		// Lock to prevent parallel test execution from causing collection modification errors
		// in LSCore's static assembly scanning during initialization
		lock (Lock)
		{
			builder.AddLSCoreDependencyInjection("TD.Web.Public");
			host = builder.Build();
			host.UseLSCoreDependencyInjection();
		}

		_dbContext = webDbContext;
	}
}
