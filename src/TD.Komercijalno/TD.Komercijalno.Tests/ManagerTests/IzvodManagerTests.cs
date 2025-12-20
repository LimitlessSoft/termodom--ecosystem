using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class IzvodManagerTests : TestBase
{
	private readonly IzvodManager _manager;

	public IzvodManagerTests()
	{
		var loggerMock = new Mock<ILogger<IzvodManager>>();
		_manager = new IzvodManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetMultiple_ReturnsFiltered()
	{
		// Arrange
		_dbContext.Izvodi.AddRange(
			new Izvod
			{
				VrDok = 1,
				BrDok = 1,
				PPID = 1,
				PozivNaBroj = "1",
				PoPdvBroj = "1",
				SifraPlacanja = "1",
				Valuta = "RSD",
			},
			new Izvod
			{
				VrDok = 1,
				BrDok = 2,
				PPID = 2,
				PozivNaBroj = "2",
				PoPdvBroj = "2",
				SifraPlacanja = "1",
				Valuta = "RSD",
			}
		);
		_dbContext.SaveChanges();

		var request = new IzvodGetMultipleRequest { PPID = new[] { 1 } };

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(1);
	}
}
