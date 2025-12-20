using FluentAssertions;
using FluentValidation;
using LSCore.DependencyInjection;
using LSCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Domain.Validators;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class DokumentManagerTests : TestBase
{
	private readonly Mock<IDokumentRepository> _repositoryMock;
	private readonly DokumentManager _manager;

	public DokumentManagerTests()
	{
		_repositoryMock = new Mock<IDokumentRepository>();
		_manager = new DokumentManager(_dbContext, _repositoryMock.Object);
	}

	[Fact]
	public void NextLinked_EmptyDatabase_ReturnsZeros()
	{
		// Arrange
		var request = new DokumentNextLinkedRequest { MagacinId = 1 };

		// Act
		var result = _manager.NextLinked(request);

		// Assert
		result.Should().Be("0000000000");
	}

	[Fact]
	public void NextLinked_ExistingDocuments_ReturnsMaxLinkedPlusOne()
	{
		// Arrange
		var magacinId = (short)1;
		_dbContext.Dokumenti.AddRange(
			new Dokument
			{
				VrDok = 1,
				BrDok = 1,
				MagacinId = magacinId,
				Linked = "0000000001",
				Valuta = "RSD",
			},
			new Dokument
			{
				VrDok = 1,
				BrDok = 2,
				MagacinId = magacinId,
				Linked = "0000000005",
				Valuta = "RSD",
			},
			new Dokument
			{
				VrDok = 1,
				BrDok = 3,
				MagacinId = (short)2,
				Linked = "0000000010",
				Valuta = "RSD",
			}
		);
		_dbContext.SaveChanges();

		var request = new DokumentNextLinkedRequest { MagacinId = magacinId };

		// Act
		var result = _manager.NextLinked(request);

		// Assert
		result.Should().Be("0000000006");
	}

	[Fact]
	public void NextLinked_Ignores9999999999()
	{
		// Arrange
		var magacinId = (short)1;
		_dbContext.Dokumenti.AddRange(
			new Dokument
			{
				VrDok = 1,
				BrDok = 1,
				MagacinId = magacinId,
				Linked = "0000000001",
				Valuta = "RSD",
			},
			new Dokument
			{
				VrDok = 1,
				BrDok = 2,
				MagacinId = magacinId,
				Linked = "9999999999",
				Valuta = "RSD",
			}
		);
		_dbContext.SaveChanges();

		var request = new DokumentNextLinkedRequest { MagacinId = magacinId };

		// Act
		var result = _manager.NextLinked(request);

		// Assert
		result.Should().Be("0000000002");
	}

	[Fact]
	public void Get_ExistingDocument_ReturnsDto()
	{
		// Arrange
		var dokument = new Dokument
		{
			VrDok = 1,
			BrDok = 100,
			MagacinId = 1,
			Valuta = "RSD",
		};
		_dbContext.Dokumenti.Add(dokument);
		_dbContext.SaveChanges();

		var request = new DokumentGetRequest { VrDok = 1, BrDok = 100 };

		// Act
		var result = _manager.Get(request);

		// Assert
		result.Should().NotBeNull();
		result.VrDok.Should().Be(1);
		result.BrDok.Should().Be(100);
	}

	[Fact]
	public void Get_NonExistingDocument_ThrowsNotFound()
	{
		// Arrange
		var request = new DokumentGetRequest { VrDok = 1, BrDok = 999 };

		// Act
		Action act = () => _manager.Get(request);

		// Assert
		act.Should().Throw<LSCoreNotFoundException>();
	}

	[Fact]
	public void Create_UsesVrstaDokMag_WhenExists()
	{
		// Arrange
		var vrDok = 1;
		var magacinId = (short)10;
		_dbContext.VrstaDokMag.Add(
			new VrstaDokMag
			{
				VrDok = vrDok,
				MagacinId = magacinId,
				Poslednji = 50,
			}
		);
		_dbContext.Magacini.Add(
			new Magacin
			{
				Id = magacinId,
				MtId = "5",
				Naziv = "Test Magacin",
			}
		);
		_dbContext.SaveChanges();

		var request = new DokumentCreateRequest
		{
			VrDok = vrDok,
			MagacinId = magacinId,
			Valuta = "RSD",
			ZapId = 1,
			RefId = 1,
		};

		// Act
		var result = _manager.Create(request);

		// Assert
		result.BrDok.Should().Be(51);
	}

	[Fact]
	public void Create_UsesVrstaDok_WhenVrstaDokMagMissing()
	{
		// Arrange
		var vrDok = 2;
		var magacinId = (short)20;
		_dbContext.VrstaDok.Add(
			new VrstaDok
			{
				Id = vrDok,
				Poslednji = 100,
				NazivDok = "Test",
			}
		);
		_dbContext.Magacini.Add(
			new Magacin
			{
				Id = magacinId,
				MtId = "5",
				Naziv = "Test",
			}
		);
		_dbContext.SaveChanges();

		var request = new DokumentCreateRequest
		{
			VrDok = vrDok,
			MagacinId = magacinId,
			Valuta = "RSD",
			ZapId = 1,
			RefId = 1,
		};

		// Act
		var result = _manager.Create(request);

		// Assert
		result.BrDok.Should().Be(101);
	}

	[Fact]
	public void Create_InvalidRequest_ThrowsBadRequest()
	{
		// Arrange
		var request = new DokumentCreateRequest
		{
			VrDok = 0, // Invalid according to DokumentCreateRequestValidator
			MagacinId = 0, // Invalid
		};

		// Act
		Action act = () => _manager.Create(request);

		// Assert
		act.Should().Throw<LSCoreBadRequestException>();
	}

	[Fact]
	public void SetNacinPlacanja_UpdatesDocument()
	{
		// Arrange
		var dokument = new Dokument
		{
			VrDok = 1,
			BrDok = 1,
			Valuta = "RSD",
		};
		_dbContext.Dokumenti.Add(dokument);
		_dbContext.SaveChanges();

		var request = new DokumentSetNacinPlacanjaRequest
		{
			VrDok = 1,
			BrDok = 1,
			NUID = 10,
		};

		// Act
		_manager.SetNacinPlacanja(request);

		// Assert
		dokument.NuId.Should().Be(10);
		_repositoryMock.Verify(r => r.Update(It.IsAny<Dokument>()), Times.Once);
	}

	[Fact]
	public void SetDokOut_UpdatesDocument()
	{
		// Arrange
		var dokument = new Dokument
		{
			VrDok = 1,
			BrDok = 1,
			Valuta = "RSD",
		};
		_dbContext.Dokumenti.Add(dokument);
		_dbContext.SaveChanges();

		var request = new DokumentSetDokOutRequest
		{
			VrDok = 1,
			BrDok = 1,
			VrDokOut = 2,
			BrDokOut = 200,
		};

		// Act
		_manager.SetDokOut(request);

		// Assert
		dokument.VrdokOut.Should().Be(2);
		dokument.BrdokOut.Should().Be(200);
		_repositoryMock.Verify(r => r.Update(It.IsAny<Dokument>()), Times.Once);
	}

	[Fact]
	public void SetFlag_UpdatesDocument()
	{
		// Arrange
		var dokument = new Dokument
		{
			VrDok = 1,
			BrDok = 1,
			Valuta = "RSD",
		};
		_dbContext.Dokumenti.Add(dokument);
		_dbContext.SaveChanges();

		var request = new DokumentSetFlagRequest
		{
			VrDok = 1,
			BrDok = 1,
			Flag = 7,
		};

		// Act
		_manager.SetFlag(request);

		// Assert
		dokument.Flag.Should().Be(7);
		_repositoryMock.Verify(r => r.Update(It.IsAny<Dokument>()), Times.Once);
	}

	[Fact]
	public void GetMultiple_WithFilters_ReturnsFilteredResults()
	{
		// Arrange
		_dbContext.Dokumenti.AddRange(
			new Dokument
			{
				VrDok = 1,
				BrDok = 1,
				MagacinId = 1,
				Datum = new DateTime(2023, 1, 1),
				Valuta = "RSD",
			},
			new Dokument
			{
				VrDok = 2,
				BrDok = 1,
				MagacinId = 2,
				Datum = new DateTime(2023, 2, 1),
				Valuta = "RSD",
			},
			new Dokument
			{
				VrDok = 1,
				BrDok = 2,
				MagacinId = 2,
				Datum = new DateTime(2023, 3, 1),
				Valuta = "RSD",
			}
		);
		_dbContext.SaveChanges();

		var request = new DokumentGetMultipleRequest { VrDok = new[] { 1 }, MagacinId = 2 };

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(1);
		result[0].VrDok.Should().Be(1);
		result[0].MagacinId.Should().Be(2);
	}
}
