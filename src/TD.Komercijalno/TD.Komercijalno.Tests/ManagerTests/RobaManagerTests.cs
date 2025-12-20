using FluentAssertions;
using LSCore.Common.Contracts;
using LSCore.DependencyInjection;
using LSCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class RobaManagerTests : TestBase
{
	private readonly Mock<IRobaRepository> _robaRepositoryMock;
	private readonly RobaManager _manager;

	public RobaManagerTests()
	{
		_robaRepositoryMock = new Mock<IRobaRepository>();
		_manager = new RobaManager(_dbContext, _robaRepositoryMock.Object);
	}

	[Fact]
	public void Create_ValidRequest_CallsInsert()
	{
		// Arrange
		var request = new RobaCreateRequest
		{
			Naziv = "Test Roba",
			KatBr = "K1",
			KatBrPro = "KP1",
			GrupaId = "G1",
			JM = "KOM",
			TarifaId = "T1",
		};

		// Act
		var result = _manager.Create(request);

		// Assert
		result.Should().NotBeNull();
		result.Naziv.Should().Be("Test Roba");
		_robaRepositoryMock.Verify(r => r.Insert(It.IsAny<Roba>()), Times.Once);
	}

	[Fact]
	public void Get_ExistingRoba_ReturnsDto()
	{
		// Arrange
		var tarifa = new Tarifa
		{
			TafiraId = "T1",
			Stopa = 20,
			Naziv = "T1",
		};
		var roba = new Roba
		{
			Id = 1,
			Naziv = "Test",
			Tarifa = tarifa,
			KatBr = "K",
			KatBrPro = "KP",
			GrupaId = "G",
			JM = "J",
			AltJM = "A",
			NazivZaCarinu = "N",
			JMR = "R",
			TarifaId = "T1",
		};
		_dbContext.Roba.Add(roba);
		_dbContext.SaveChanges();

		var request = new LSCoreIdRequest { Id = 1 };

		// Act
		var result = _manager.Get(request);

		// Assert
		result.Should().NotBeNull();
		result.RobaId.Should().Be(1);
		result.Tarifa.Should().NotBeNull();
	}

	[Fact]
	public void Get_NonExistingRoba_ThrowsNotFound()
	{
		// Arrange
		var request = new LSCoreIdRequest { Id = 999 };

		// Act
		Action act = () => _manager.Get(request);

		// Assert
		act.Should().Throw<LSCoreNotFoundException>();
	}

	[Fact]
	public void GetMultiple_ReturnsResults()
	{
		// Arrange
		var tarifa = new Tarifa
		{
			TafiraId = "T1",
			Stopa = 20,
			Naziv = "T1",
		};
		_dbContext.Roba.AddRange(
			new Roba
			{
				Id = 1,
				Naziv = "R1",
				Vrsta = 1,
				Tarifa = tarifa,
				KatBr = "K",
				KatBrPro = "KP",
				GrupaId = "G",
				JM = "J",
				AltJM = "A",
				NazivZaCarinu = "N",
				JMR = "R",
				TarifaId = "T1",
			},
			new Roba
			{
				Id = 2,
				Naziv = "R2",
				Vrsta = 2,
				Tarifa = tarifa,
				KatBr = "K",
				KatBrPro = "KP",
				GrupaId = "G",
				JM = "J",
				AltJM = "A",
				NazivZaCarinu = "N",
				JMR = "R",
				TarifaId = "T1",
			}
		);
		_dbContext.SaveChanges();

		var request = new RobaGetMultipleRequest { Vrsta = 1 };

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(1);
		result[0].RobaId.Should().Be(1);
	}

	[Fact]
	public void Create_InvalidRequest_ThrowsBadRequest()
	{
		// Arrange
		var request = new RobaCreateRequest
		{
			Naziv = "", // Invalid
		};

		// Act
		Action act = () => _manager.Create(request);

		// Assert
		act.Should().Throw<LSCoreBadRequestException>();
	}
}
