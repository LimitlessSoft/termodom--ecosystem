using FluentAssertions;
using LSCore.DependencyInjection;
using LSCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class StavkaManagerTests
{
	private static readonly object Lock = new();
	private readonly Mock<ILogger<StavkaManager>> _loggerMock;
	private readonly KomercijalnoDbContext _dbContext;
	private readonly Mock<IDokumentRepository> _dokumentRepositoryMock;
	private readonly Mock<IMagacinRepository> _magacinRepositoryMock;
	private readonly Mock<IRobaRepository> _robaRepositoryMock;
	private readonly Mock<IStavkaRepository> _stavkaRepositoryMock;
	private readonly Mock<IProcedureManager> _procedureManagerMock;
	private readonly StavkaManager _manager;

	public StavkaManagerTests()
	{
		var builder = Host.CreateApplicationBuilder();

		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		builder.Services.AddScoped<KomercijalnoDbContext>(_ => new KomercijalnoDbContext(options));
		builder.Services.AddScoped<DbContext>(_ => new KomercijalnoDbContext(options));
		builder.Services.AddLogging();

		lock (Lock)
		{
			builder.AddLSCoreDependencyInjection("TD.Komercijalno");
		}

		var host = builder.Build();
		host.UseLSCoreDependencyInjection();

		_dbContext = new KomercijalnoDbContext(options);
		_loggerMock = new Mock<ILogger<StavkaManager>>();
		_dokumentRepositoryMock = new Mock<IDokumentRepository>();
		_magacinRepositoryMock = new Mock<IMagacinRepository>();
		_robaRepositoryMock = new Mock<IRobaRepository>();
		_stavkaRepositoryMock = new Mock<IStavkaRepository>();
		_procedureManagerMock = new Mock<IProcedureManager>();

		_manager = new StavkaManager(
			_loggerMock.Object,
			_dbContext,
			_dokumentRepositoryMock.Object,
			_magacinRepositoryMock.Object,
			_robaRepositoryMock.Object,
			_stavkaRepositoryMock.Object,
			_procedureManagerMock.Object
		);
	}

	private Roba CreateValidRoba(int id, string naziv, double stopa = 20)
	{
		return new Roba
		{
			Id = id,
			Naziv = naziv,
			KatBr = "KB" + id,
			KatBrPro = "KBP" + id,
			GrupaId = "G1",
			JM = "kom",
			AltJM = "kom",
			NazivZaCarinu = naziv,
			TarifaId = "T" + id,
			Tarifa = new Tarifa
			{
				TafiraId = "T" + id,
				Stopa = stopa,
				Naziv = "Tarifa " + id,
			},
			JMR = "kom",
		};
	}

	[Fact]
	public void Create_ValidRequest_ReturnsDto()
	{
		// Arrange
		var vrDok = 1;
		var brDok = 100;
		var magacinId = (short)1;
		var robaId = 500;

		var dokument = new Dokument
		{
			VrDok = vrDok,
			BrDok = brDok,
			MagacinId = magacinId,
			Kurs = 1,
			Valuta = "RSD",
		};
		var magacin = new Magacin
		{
			Id = magacinId,
			MtId = "MT1",
			Vrsta = MagacinVrsta.Veleprodajni,
			Naziv = "Test Magacin",
		};
		var roba = CreateValidRoba(robaId, "Test Roba", 20);

		_dokumentRepositoryMock.Setup(r => r.Get(vrDok, brDok)).Returns(dokument);
		_magacinRepositoryMock.Setup(r => r.Get(magacinId)).Returns(magacin);
		_robaRepositoryMock
			.Setup(r =>
				r.Get(
					robaId,
					It.IsAny<System.Linq.Expressions.Expression<System.Func<Roba, object>>[]>()
				)
			)
			.Returns(roba);
		_procedureManagerMock
			.Setup(m => m.GetProdajnaCenaNaDan(It.IsAny<ProceduraGetProdajnaCenaNaDanRequest>()))
			.Returns(120);

		_dbContext.Magacini.Add(magacin);
		_dbContext.SaveChanges();

		var request = new StavkaCreateRequest
		{
			VrDok = vrDok,
			BrDok = brDok,
			RobaId = robaId,
			Kolicina = 10,
			Rabat = 0,
		};

		// Act
		var result = _manager.Create(request);

		// Assert
		result.Should().NotBeNull();
		result.RobaId.Should().Be(robaId);
		result.Naziv.Should().Be("Test Roba");
		result.ProdajnaCena.Should().Be(100); // 120 / 1.2 = 100
		_stavkaRepositoryMock.Verify(r => r.Insert(It.IsAny<Stavka>()), Times.Once);
	}

	[Fact]
	public void Create_CalculatesRabatCorrecty()
	{
		// Arrange
		var vrDok = 1;
		var brDok = 100;
		var magacinId = (short)1;
		var robaId = 500;

		var dokument = new Dokument
		{
			VrDok = vrDok,
			BrDok = brDok,
			MagacinId = magacinId,
			Kurs = 1,
			Valuta = "RSD",
		};
		var magacin = new Magacin
		{
			Id = magacinId,
			MtId = "MT1",
			Vrsta = MagacinVrsta.Veleprodajni,
			Naziv = "Test Magacin",
		};
		var roba = CreateValidRoba(robaId, "Test Roba", 0);

		_dokumentRepositoryMock.Setup(r => r.Get(vrDok, brDok)).Returns(dokument);
		_magacinRepositoryMock.Setup(r => r.Get(magacinId)).Returns(magacin);
		_robaRepositoryMock
			.Setup(r =>
				r.Get(
					robaId,
					It.IsAny<System.Linq.Expressions.Expression<System.Func<Roba, object>>[]>()
				)
			)
			.Returns(roba);
		_procedureManagerMock
			.Setup(m => m.GetProdajnaCenaNaDan(It.IsAny<ProceduraGetProdajnaCenaNaDanRequest>()))
			.Returns(100);

		_dbContext.Magacini.Add(magacin);
		_dbContext.SaveChanges();

		var request = new StavkaCreateRequest
		{
			VrDok = vrDok,
			BrDok = brDok,
			RobaId = robaId,
			ProdajnaCenaBezPdv = 80, // 20% discount
			Kolicina = 1,
			Rabat = 0,
		};

		// Act
		var result = _manager.Create(request);

		// Assert
		result.Rabat.Should().BeApproximately(20, 0.0001);
	}

	[Fact]
	public void GetMultiple_ReturnsFilteredStavke()
	{
		// Arrange
		_dbContext.Stavke.AddRange(
			new Stavka
			{
				VrDok = 1,
				BrDok = 1,
				RobaId = 1,
				MagacinId = 1,
			},
			new Stavka
			{
				VrDok = 1,
				BrDok = 1,
				RobaId = 2,
				MagacinId = 1,
			},
			new Stavka
			{
				VrDok = 1,
				BrDok = 2,
				RobaId = 1,
				MagacinId = 1,
			}
		);
		_dbContext.SaveChanges();

		var request = new StavkaGetMultipleRequest
		{
			VrDok = new long[] { 1 },
			Dokument = new string[] { "1-1" },
		};

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(2);
	}

	[Fact]
	public void CreateOptimized_ValidRequest_InsertsMultipleStavke()
	{
		// Arrange
		var vrDok = 1;
		var brDok = 100;
		var magacinId = (short)1;
		var robaId1 = 501;
		var robaId2 = 502;

		var dokument = new Dokument
		{
			VrDok = vrDok,
			BrDok = brDok,
			MagacinId = magacinId,
			Kurs = 1,
			Valuta = "RSD",
		};
		var magacin = new Magacin
		{
			Id = magacinId,
			MtId = "MT1",
			Vrsta = MagacinVrsta.Veleprodajni,
			Naziv = "Test",
		};
		var roba1 = CreateValidRoba(robaId1, "Roba 1", 20);
		var roba2 = CreateValidRoba(robaId2, "Roba 2", 20);

		_dokumentRepositoryMock.Setup(r => r.Get(vrDok, brDok)).Returns(dokument);
		_magacinRepositoryMock.Setup(r => r.Get(magacinId)).Returns(magacin);

		_dbContext.Roba.AddRange(roba1, roba2);
		_dbContext.Magacini.Add(magacin);
		_dbContext.SaveChanges();

		_procedureManagerMock
			.Setup(m =>
				m.GetProdajnaCenaNaDanOptimized(
					It.IsAny<ProceduraGetProdajnaCenaNaDanOptimizedRequest>()
				)
			)
			.Returns(
				new List<ProdajnaCenaNaDanDto>
				{
					new() { RobaId = robaId1, ProdajnaCenaBezPDV = 100 },
					new() { RobaId = robaId2, ProdajnaCenaBezPDV = 200 },
				}
			);

		var request = new StavkeCreateOptimizedRequest
		{
			Stavke = new List<StavkaCreateRequest>
			{
				new()
				{
					VrDok = vrDok,
					BrDok = brDok,
					RobaId = robaId1,
					Kolicina = 1,
					Rabat = 0,
				},
				new()
				{
					VrDok = vrDok,
					BrDok = brDok,
					RobaId = robaId2,
					Kolicina = 2,
					Rabat = 0,
				},
			},
		};

		// Act
		var result = _manager.CreateOptimized(request);

		// Assert
		result.Should().HaveCount(2);
		_stavkaRepositoryMock.Verify(r => r.InsertRange(It.IsAny<List<Stavka>>()), Times.Once);
	}

	[Fact]
	public void Create_InvalidRequest_ThrowsBadRequest()
	{
		// Arrange
		var request = new StavkaCreateRequest
		{
			VrDok = 0, // Invalid
		};

		// Act
		Action act = () => _manager.Create(request);

		// Assert
		act.Should().Throw<LSCoreBadRequestException>();
	}
}
