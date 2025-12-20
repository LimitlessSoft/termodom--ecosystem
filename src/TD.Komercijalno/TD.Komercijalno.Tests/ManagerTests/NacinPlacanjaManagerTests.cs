using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class NacinPlacanjaManagerTests
{
	private readonly KomercijalnoDbContext _dbContext;
	private readonly NacinPlacanjaManager _manager;

	public NacinPlacanjaManagerTests()
	{
		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_dbContext = new KomercijalnoDbContext(options);
		var loggerMock = new Mock<ILogger<NacinPlacanjaManager>>();
		_manager = new NacinPlacanjaManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetMultiple_ReturnsAll()
	{
		// Arrange
		_dbContext.NaciniPlacanja.AddRange(
			new NacinPlacanja { NUID = 1, Naziv = "N1" },
			new NacinPlacanja { NUID = 2, Naziv = "N2" }
		);
		_dbContext.SaveChanges();

		// Act
		var result = _manager.GetMultiple();

		// Assert
		result.Should().HaveCount(2);
	}
}
