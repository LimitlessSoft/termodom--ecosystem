using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class DokumentManagerTests
{
	private readonly KomercijalnoDbContext _dbContext;
	private readonly Mock<IDokumentRepository> _repositoryMock;
	private readonly DokumentManager _manager;

	public DokumentManagerTests()
	{
		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_dbContext = new KomercijalnoDbContext(options);
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
}
