using FluentAssertions;
using LSCore.DependencyInjection;
using LSCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class KomentarManagerTests : TestBase
{
	private readonly Mock<IKomentarRepository> _repositoryMock;
	private readonly KomentarManager _manager;

	public KomentarManagerTests()
	{
		_repositoryMock = new Mock<IKomentarRepository>();
		var loggerMock = new Mock<ILogger<KomentarManager>>();
		_manager = new KomentarManager(loggerMock.Object, _dbContext, _repositoryMock.Object);
	}

	[Fact]
	public void Create_CallsInsert()
	{
		// Arrange
		var request = new CreateKomentarRequest
		{
			VrDok = 1,
			BrDok = 1,
			Komentar = "Test",
		};

		// Act
		var result = _manager.Create(request);

		// Assert
		result.Should().NotBeNull();
		_repositoryMock.Verify(r => r.Insert(It.IsAny<Komentar>()), Times.Once);
	}

	[Fact]
	public void Get_CallsRepository()
	{
		// Arrange
		var request = new GetKomentarRequest { VrDok = 1, BrDok = 1 };
		_repositoryMock
			.Setup(r => r.Get(1, 1))
			.Returns(
				new Komentar
				{
					VrDok = 1,
					BrDok = 1,
					JavniKomentar = "C",
				}
			);

		// Act
		var result = _manager.Get(request);

		// Assert
		result.Should().NotBeNull();
		result.JavniKomentar.Should().Be("C");
	}

	[Fact]
	public void Create_InvalidRequest_ThrowsBadRequest()
	{
		// Arrange
		var request = new CreateKomentarRequest
		{
			VrDok = 0, // Invalid
			Komentar = "", // Invalid
		};

		// Act
		Action act = () => _manager.Create(request);

		// Assert
		act.Should().Throw<LSCoreBadRequestException>();
	}
}
