using FluentAssertions;
using LSCore.Common.Contracts;
using LSCore.DependencyInjection;
using LSCore.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class PartnerManagerTests
{
	private static readonly object Lock = new();
	private readonly KomercijalnoDbContext _dbContext;
	private readonly PartnerManager _manager;

	public PartnerManagerTests()
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
		_manager = new PartnerManager(_dbContext);
	}

	private Partner CreateValidPartner(int ppid, string naziv)
	{
		return new Partner
		{
			Ppid = ppid,
			Naziv = naziv,
			DelatnostId = "0",
			Gln = "",
			KontoIzvod = "",
			KontoTrosak = "",
			Mbroj = "",
			MestoId = "0",
			Mobilni = "",
			Nadimak = "",
			NasGln = "",
			NazivZaStampu = naziv,
			Pib = "",
			Rbroj = "",
			Sdel = "",
			Valuta = "DIN",
			WebKorisnik = "",
			WebShopPass = "",
			WebShopUser = "",
		};
	}

	[Fact]
	public void Create_IncrementsMaxPpid()
	{
		// Arrange
		_dbContext.Partneri.Add(CreateValidPartner(100, "P1"));
		_dbContext.SaveChanges();

		var request = new PartneriCreateRequest
		{
			Naziv = "New Partner",
			Adresa = "Test Adresa",
			Posta = "11000",
			Mesto = "Beograd",
			Email = "test@test.com",
			Kontakt = "Test Kontakt",
			Mbroj = "12345678",
			MestoId = "1",
			Pib = "123456789",
			Mobilni = "0641234567",
			UPdvSistemu = true,
		};

		// Act
		var resultPpid = _manager.Create(request);

		// Assert
		resultPpid.Should().Be(101);
		var partner = _dbContext.Partneri.FirstOrDefault(x => x.Ppid == 101);
		partner.Should().NotBeNull();
		partner.Naziv.Should().Be("New Partner");
		partner.Pdvo.Should().Be(1);
	}

	[Fact]
	public void GetSingle_ExistingPartner_ReturnsDto()
	{
		// Arrange
		_dbContext.Partneri.Add(CreateValidPartner(5, "Test Partner"));
		_dbContext.SaveChanges();

		var request = new LSCoreIdRequest { Id = 5 };

		// Act
		var result = _manager.GetSingle(request);

		// Assert
		result.Should().NotBeNull();
		result.Ppid.Should().Be(5);
		result.Naziv.Should().Be("Test Partner");
	}

	[Fact]
	public void GetSingle_NonExistingPartner_ThrowsNotFound()
	{
		// Arrange
		var request = new LSCoreIdRequest { Id = 999 };

		// Act
		Action act = () => _manager.GetSingle(request);

		// Assert
		act.Should().Throw<LSCoreNotFoundException>();
	}

	[Fact]
	public void GetDuplikat_ReturnsTrue_WhenPibMatches()
	{
		// Arrange
		var partner = CreateValidPartner(1, "P1");
		partner.Pib = "123456789";
		_dbContext.Partneri.Add(partner);
		_dbContext.SaveChanges();

		var request = new PartneriGetDuplikatRequest { Pib = "123456789" };

		// Act
		var result = _manager.GetDuplikat(request);

		// Assert
		result.Should().BeTrue();
	}

	[Fact]
	public void Create_InvalidRequest_ThrowsBadRequest()
	{
		// Arrange
		var request = new PartneriCreateRequest
		{
			Naziv = "", // Invalid
		};

		// Act
		Action act = () => _manager.Create(request);

		// Assert
		act.Should().Throw<LSCoreBadRequestException>();
	}
}
