using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class IstorijaUplataManagerTests
{
	private readonly KomercijalnoDbContext _dbContext;
	private readonly IstorijaUplataManager _manager;

	public IstorijaUplataManagerTests()
	{
		var options = new DbContextOptionsBuilder<KomercijalnoDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		_dbContext = new KomercijalnoDbContext(options);
		_manager = new IstorijaUplataManager(_dbContext);
	}

	[Fact]
	public void GetMultiple_ReturnsFiltered()
	{
		// Arrange
		_dbContext.IstorijaUplata.AddRange(
			new IstorijaUplata
			{
				VrDok = 1,
				BrDok = 1,
				Datum = DateTime.Now,
				PPID = 1,
				Valuta = "RSD",
			},
			new IstorijaUplata
			{
				VrDok = 1,
				BrDok = 2,
				Datum = DateTime.Now,
				PPID = 2,
				Valuta = "RSD",
			}
		);
		_dbContext.SaveChanges();

		var request =
			new TD.Komercijalno.Contracts.Requests.IstorijaUplata.IstorijaUplataGetMultipleRequest
			{
				PPID = new[] { 1 },
			};

		// Act
		var result = _manager.GetMultiple(request);

		// Assert
		result.Should().HaveCount(1);
	}
}
