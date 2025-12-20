using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class NamenaManagerTests
{
	private readonly KomercijalnoDbContext _dbContext;
	private readonly NamenaManager _manager;

	public NamenaManagerTests()
	{
		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_dbContext = new KomercijalnoDbContext(options);
		var loggerMock = new Mock<ILogger<NamenaManager>>();
		_manager = new NamenaManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetMultiple_ReturnsAll()
	{
		// Arrange
		_dbContext.Namene.AddRange(
			new Namena
			{
				Id = 1,
				Naziv = "N1",
				Napomena = "N1",
			},
			new Namena
			{
				Id = 2,
				Naziv = "N2",
				Napomena = "N2",
			}
		);
		_dbContext.SaveChanges();

		// Act
		var result = _manager.GetMultiple();

		// Assert
		result.Should().HaveCount(2);
	}
}
