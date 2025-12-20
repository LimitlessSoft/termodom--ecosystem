using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Magacini;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class MagacinManagerTests
{
	private readonly KomercijalnoDbContext _dbContext;
	private readonly MagacinManager _manager;

	public MagacinManagerTests()
	{
		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_dbContext = new KomercijalnoDbContext(options);
		var loggerMock = new Mock<ILogger<MagacinManager>>();
		_manager = new MagacinManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetMultiple_FilterByVrsta_ReturnsFiltered()
	{
		// Arrange
		_dbContext.Magacini.AddRange(
			new Magacin
			{
				Id = 1,
				Naziv = "M1",
				Vrsta = MagacinVrsta.Veleprodajni,
				MtId = "1",
			},
			new Magacin
			{
				Id = 2,
				Naziv = "M2",
				Vrsta = MagacinVrsta.Maloprodajni,
				MtId = "2",
			}
		);
		_dbContext.SaveChanges();

		var request = new MagaciniGetMultipleRequest
		{
			Vrsta = new[] { MagacinVrsta.Veleprodajni },
		};

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(1);
		result[0].MagacinId.Should().Be(1);
	}
}
