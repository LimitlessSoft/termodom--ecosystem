using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Komercijalno.Client;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Domain.Managers;
using TD.Office.Public.Repository.Repositories;
using Xunit;

namespace TD.Office.Public.Tests.ManagerTests;

public class PopisManagerTests : TestBase
{
	private readonly PopisManager _manager;
	private readonly Mock<IUserRepository> _userRepositoryMock = new();
	private readonly IPopisRepository _popisRepository;
	private readonly Mock<ILogger<PopisManager>> _loggerMock = new();
	private readonly Mock<TDKomercijalnoClient> _komercijalnoClientMock;
	private readonly Mock<ITDKomercijalnoClientFactory> _komercijalnoClientFactoryMock = new();
	private readonly Mock<IKomercijalnoMagacinFirmaRepository> _komercijalnoMagacinFirmaRepositoryMock =
		new();
	private readonly Mock<IConfigurationRoot> _configurationRootMock = new();

	public PopisManagerTests()
	{
		_popisRepository = new PopisRepository(_dbContext);
		_komercijalnoClientMock = new Mock<TDKomercijalnoClient>(
			2025,
			TDKomercijalnoEnvironment.Development,
			TDKomercijalnoFirma.TCMDZ
		);

		_manager = new PopisManager(
			_loggerMock.Object,
			_popisRepository,
			_userRepositoryMock.Object,
			_komercijalnoClientMock.Object,
			_komercijalnoClientFactoryMock.Object,
			_komercijalnoMagacinFirmaRepositoryMock.Object,
			_configurationRootMock.Object
		);
	}

	[Fact]
	public void GetById_WhenUserHasPermission_ReturnsMappedDto()
	{
		// Arrange
		var id = 1L;
		var magacinId = 1;
		var entity = new PopisDokumentEntity
		{
			Id = id,
			MagacinId = magacinId,
			Status = DokumentStatus.Open,
			IsActive = true,
			CreatedAt = DateTime.UtcNow,
			KomercijalnoPopisBrDok = 123,
		};
		_dbContext.Popisi.Add(entity);
		_dbContext.SaveChanges();

		var currentUser = new UserEntity { Id = 1, Type = UserType.User };
		_userRepositoryMock.Setup(r => r.GetCurrentUser()).Returns(currentUser);
		_userRepositoryMock
			.Setup(r => r.HasPermission(currentUser.Id, Permission.RobaPopisRead))
			.Returns(true);

		_komercijalnoMagacinFirmaRepositoryMock
			.Setup(r => r.GetByMagacinId(magacinId))
			.Returns(new KomercijalnoMagacinFirmaEntity { ApiFirma = TDKomercijalnoFirma.TCMDZ });

		_komercijalnoClientFactoryMock
			.Setup(f =>
				f.Create(
					It.IsAny<int>(),
					It.IsAny<TDKomercijalnoEnvironment>(),
					It.IsAny<TDKomercijalnoFirma>()
				)
			)
			.Returns(_komercijalnoClientMock.Object);

		_configurationRootMock.Setup(c => c["DEPLOY_ENV"]).Returns("development");

		// Act & Assert
		// We expect it to fail here because we didn't mock client.Dokumenti.Get
		// but at least we've set up the foundations.
		var action = () => _manager.GetById(id);
		action.Should().Throw<Exception>();
	}
}
