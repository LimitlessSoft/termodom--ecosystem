using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Promene;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class PromenaManagerTests
{
	private readonly KomercijalnoDbContext _dbContext;
	private readonly PromenaManager _manager;

	public PromenaManagerTests()
	{
		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_dbContext = new KomercijalnoDbContext(options);
		var loggerMock = new Mock<ILogger<PromenaManager>>();
		_manager = new PromenaManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetMultiple_ReturnsFiltered()
	{
		// Arrange
		_dbContext.Promene.AddRange(
			new Promena
			{
				Konto = "1200",
				PPID = 1,
				BrojNaloga = "1",
			},
			new Promena
			{
				Konto = "1300",
				PPID = 2,
				BrojNaloga = "2",
			}
		);
		_dbContext.SaveChanges();

		var request = new PromenaGetMultipleRequest { KontoStartsWith = "12" };

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(1);
		result[0].Konto.Should().Be("1200");
	}
}
