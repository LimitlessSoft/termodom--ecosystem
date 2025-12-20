using FluentAssertions;
using LSCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class RobaUMagacinuManagerTests : TestBase
{
	private readonly RobaUMagacinuManager _manager;

	public RobaUMagacinuManagerTests()
	{
		var loggerMock = new Mock<ILogger<RobaUMagacinuManager>>();
		_manager = new RobaUMagacinuManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetMultiple_ReturnsFiltered()
	{
		// Arrange
		_dbContext.RobaUMagacinu.AddRange(
			new RobaUMagacinu { MagacinId = 1, RobaId = 100 },
			new RobaUMagacinu { MagacinId = 2, RobaId = 100 }
		);
		_dbContext.SaveChanges();

		var request = new RobaUMagacinuGetMultipleRequest { MagacinId = 1 };

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(1);
		result[0].MagacinId.Should().Be(1);
	}
}
