using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class VrstaDokManagerTests
{
	private readonly KomercijalnoDbContext _dbContext;
	private readonly VrstaDokManager _manager;

	public VrstaDokManagerTests()
	{
		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_dbContext = new KomercijalnoDbContext(options);
		var loggerMock = new Mock<ILogger<VrstaDokManager>>();
		_manager = new VrstaDokManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetMultiple_ReturnsAll()
	{
		// Arrange
		_dbContext.VrstaDok.AddRange(
			new VrstaDok { Id = 1, NazivDok = "D1" },
			new VrstaDok { Id = 2, NazivDok = "D2" }
		);
		_dbContext.SaveChanges();

		// Act
		var result = _manager.GetMultiple();

		// Assert
		result.Should().HaveCount(2);
	}
}
