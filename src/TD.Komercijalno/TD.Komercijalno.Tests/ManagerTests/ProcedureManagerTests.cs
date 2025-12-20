using FluentAssertions;
using LSCore.DependencyInjection;
using LSCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class ProcedureManagerTests
{
	private static readonly object Lock = new();
	private readonly KomercijalnoDbContext _dbContext;
	private readonly ProcedureManager _manager;

	public ProcedureManagerTests()
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
		var loggerMock = new Mock<ILogger<ProcedureManager>>();
		_manager = new ProcedureManager(loggerMock.Object, _dbContext);
	}

	[Fact]
	public void GetProdajnaCenaNaDan_ReturnsCorrectPrice()
	{
		// Arrange
		var magacin = new Magacin
		{
			Id = 1,
			Naziv = "M",
			Vrsta = MagacinVrsta.Veleprodajni,
			MtId = "1",
		};
		var vrstaDok = new VrstaDok
		{
			Id = 1,
			NazivDok = "D",
			DefiniseCenu = 1,
			ImaKarticu = 1,
		};
		var dokument = new Dokument
		{
			VrDok = 1,
			BrDok = 1,
			MagacinId = 1,
			Datum = DateTime.Now,
			KodDok = 0,
			Linked = "0000000001",
			Valuta = "RSD",
		};
		var tarifa = new Tarifa
		{
			TafiraId = "1",
			Stopa = 20,
			Naziv = "T",
		};
		var stavka = new Stavka
		{
			VrDok = 1,
			BrDok = 1,
			RobaId = 100,
			MagacinId = 1,
			ProdajnaCena = 100,
			TarifaId = "1",
		};

		_dbContext.Magacini.Add(magacin);
		_dbContext.VrstaDok.Add(vrstaDok);
		_dbContext.Dokumenti.Add(dokument);
		_dbContext.Tarife.Add(tarifa);
		_dbContext.Stavke.Add(stavka);
		_dbContext.SaveChanges();

		var request = new ProceduraGetProdajnaCenaNaDanRequest
		{
			RobaId = 100,
			MagacinId = 1,
			Datum = DateTime.Now.AddDays(1),
		};

		// Act
		var result = _manager.GetProdajnaCenaNaDan(request);

		// Assert
		result.Should().Be(120); // 100 * (1 + 20/100)
	}

	[Fact]
	public void GetProdajnaCenaNaDan_InvalidRequest_ThrowsBadRequest()
	{
		// Arrange
		var request = new ProceduraGetProdajnaCenaNaDanRequest
		{
			MagacinId = 0, // Invalid
		};

		// Act
		Action act = () => _manager.GetProdajnaCenaNaDan(request);

		// Assert
		act.Should().Throw<LSCoreBadRequestException>();
	}
}
