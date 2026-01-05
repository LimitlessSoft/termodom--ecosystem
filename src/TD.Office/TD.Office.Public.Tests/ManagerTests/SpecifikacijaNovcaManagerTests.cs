using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LSCore.Auth.Contracts;
using LSCore.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.Factories;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;
using TD.Office.Public.Domain.Managers;
using Xunit;

namespace TD.Office.Public.Tests.ManagerTests;

/// <summary>
/// Tests for SpecifikacijaNovcaManager, specifically covering the bugfix
/// where GetSingleAsync was incorrectly comparing user.StoreId with request.Id
/// instead of the entity's MagacinId.
/// </summary>
public class SpecifikacijaNovcaManagerTests : TestBase
{
	private readonly SpecifikacijaNovcaManager _manager;
	private readonly Mock<IUserRepository> _userRepositoryMock = new();
	private readonly Mock<ISpecifikacijaNovcaRepository> _specifikacijaNovcaRepositoryMock = new();
	private readonly Mock<ILogger<SpecifikacijaNovcaManager>> _loggerMock = new();
	private readonly Mock<TDKomercijalnoClient> _komercijalnoClientMock;
	private readonly Mock<ITDKomercijalnoClientFactory> _komercijalnoClientFactoryMock = new();
	private readonly Mock<IKomercijalnoMagacinFirmaRepository> _komercijalnoMagacinFirmaRepositoryMock =
		new();
	private readonly Mock<IConfigurationRoot> _configurationRootMock = new();
	private readonly Mock<LSCoreAuthContextEntity<string>> _contextEntityMock = new();

	public SpecifikacijaNovcaManagerTests()
	{
		_komercijalnoClientMock = new Mock<TDKomercijalnoClient>(
			2025,
			TDKomercijalnoEnvironment.Development,
			TDKomercijalnoFirma.TCMDZ
		);

		SetupKomercijalnoClientMock();

		_manager = new SpecifikacijaNovcaManager(
			_loggerMock.Object,
			_dbContext,
			_configurationRootMock.Object,
			_komercijalnoMagacinFirmaRepositoryMock.Object,
			_contextEntityMock.Object,
			_specifikacijaNovcaRepositoryMock.Object,
			_userRepositoryMock.Object,
			_komercijalnoClientFactoryMock.Object
		);
	}

	private void SetupKomercijalnoClientMock()
	{
		var handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost/"),
		};

		handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(
				new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = JsonContent.Create(new List<DokumentDto>()),
				}
			);

		var dokumentiEndpoints = new DokumentiEndpoints(() => httpClient, _ => { });
		typeof(TDKomercijalnoClient)
			.GetProperty("Dokumenti")!
			.SetValue(_komercijalnoClientMock.Object, dokumentiEndpoints);

		_komercijalnoClientFactoryMock
			.Setup(f =>
				f.Create(
					It.IsAny<int>(),
					It.IsAny<TDKomercijalnoEnvironment>(),
					It.IsAny<TDKomercijalnoFirma>()
				)
			)
			.Returns(_komercijalnoClientMock.Object);

		_komercijalnoMagacinFirmaRepositoryMock
			.Setup(r => r.GetByMagacinId(It.IsAny<int>()))
			.Returns(new KomercijalnoMagacinFirmaEntity { ApiFirma = TDKomercijalnoFirma.TCMDZ });

		_configurationRootMock.Setup(c => c["DEPLOY_ENV"]).Returns("develop");
	}

	/// <summary>
	/// Test that verifies the bugfix: GetSingleAsync should check user.StoreId against
	/// the entity's MagacinId, NOT against request.Id.
	///
	/// Before the fix: user.StoreId (5) != request.Id (100) would throw LSCoreForbiddenException
	/// After the fix: user.StoreId (5) == response.MagacinId (5) allows access
	/// </summary>
	[Fact]
	public async Task GetSingleAsync_WhenUserStoreIdMatchesMagacinId_ReturnsData()
	{
		// Arrange
		var entityId = 100L; // This is the specifikacija ID in the request
		var magacinId = 5; // This is the store ID that should be checked
		var userStoreId = 5; // User's store matches the entity's magacin

		var entity = new SpecifikacijaNovcaEntity
		{
			Id = entityId,
			MagacinId = magacinId,
			Datum = DateTime.UtcNow,
			IsActive = true,
		};

		_specifikacijaNovcaRepositoryMock
			.Setup(r => r.Get(entityId))
			.Returns(entity);

		var currentUser = new UserEntity
		{
			Id = 1,
			Username = "testuser",
			Password = "test",
			Nickname = "Test User",
			Type = UserType.User,
			StoreId = userStoreId,
			Permissions = new List<UserPermissionEntity>(),
		};
		_userRepositoryMock.Setup(r => r.GetCurrentUser()).Returns(currentUser);

		var request = new GetSingleSpecifikacijaNovcaRequest { Id = entityId };

		// Act
		var result = await _manager.GetSingleAsync(request);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(entityId);
		result.MagacinId.Should().Be(magacinId);
	}

	/// <summary>
	/// Test that when user.StoreId does NOT match entity.MagacinId but user
	/// has SpecifikacijaNovcaSviMagacini permission, access is allowed.
	/// </summary>
	[Fact]
	public async Task GetSingleAsync_WhenUserHasSviMagaciniPermission_AllowsAccess()
	{
		// Arrange
		var entityId = 100L;
		var magacinId = 5;
		var userStoreId = 10; // Different from magacinId

		var entity = new SpecifikacijaNovcaEntity
		{
			Id = entityId,
			MagacinId = magacinId,
			Datum = DateTime.UtcNow,
			IsActive = true,
		};

		_specifikacijaNovcaRepositoryMock
			.Setup(r => r.Get(entityId))
			.Returns(entity);

		var currentUser = new UserEntity
		{
			Id = 1,
			Username = "testuser",
			Password = "test",
			Nickname = "Test User",
			Type = UserType.User,
			StoreId = userStoreId,
			Permissions = new List<UserPermissionEntity>
			{
				new()
				{
					Id = 1,
					UserId = 1,
					Permission = Permission.SpecifikacijaNovcaSviMagacini,
					IsActive = true,
				},
			},
		};
		_userRepositoryMock.Setup(r => r.GetCurrentUser()).Returns(currentUser);

		var request = new GetSingleSpecifikacijaNovcaRequest { Id = entityId };

		// Act
		var result = await _manager.GetSingleAsync(request);

		// Assert
		result.Should().NotBeNull();
		result.Id.Should().Be(entityId);
		result.MagacinId.Should().Be(magacinId);
	}

	/// <summary>
	/// Test that when user.StoreId does NOT match entity.MagacinId and user
	/// does NOT have the required permission, LSCoreForbiddenException is thrown.
	/// </summary>
	[Fact]
	public async Task GetSingleAsync_WhenUserStoreIdDifferentAndNoPermission_ThrowsForbidden()
	{
		// Arrange
		var entityId = 100L;
		var magacinId = 5;
		var userStoreId = 10; // Different from magacinId

		var entity = new SpecifikacijaNovcaEntity
		{
			Id = entityId,
			MagacinId = magacinId,
			Datum = DateTime.UtcNow,
			IsActive = true,
		};

		_specifikacijaNovcaRepositoryMock
			.Setup(r => r.Get(entityId))
			.Returns(entity);

		var currentUser = new UserEntity
		{
			Id = 1,
			Username = "testuser",
			Password = "test",
			Nickname = "Test User",
			Type = UserType.User,
			StoreId = userStoreId,
			Permissions = new List<UserPermissionEntity>(), // No permissions
		};
		_userRepositoryMock.Setup(r => r.GetCurrentUser()).Returns(currentUser);

		var request = new GetSingleSpecifikacijaNovcaRequest { Id = entityId };

		// Act
		var action = () => _manager.GetSingleAsync(request);

		// Assert
		await action.Should().ThrowAsync<LSCoreForbiddenException>();
	}

	/// <summary>
	/// Test that inactive SpecifikacijaNovcaSviMagacini permission is not honored.
	/// </summary>
	[Fact]
	public async Task GetSingleAsync_WhenUserHasInactiveSviMagaciniPermission_ThrowsForbidden()
	{
		// Arrange
		var entityId = 100L;
		var magacinId = 5;
		var userStoreId = 10; // Different from magacinId

		var entity = new SpecifikacijaNovcaEntity
		{
			Id = entityId,
			MagacinId = magacinId,
			Datum = DateTime.UtcNow,
			IsActive = true,
		};

		_specifikacijaNovcaRepositoryMock
			.Setup(r => r.Get(entityId))
			.Returns(entity);

		var currentUser = new UserEntity
		{
			Id = 1,
			Username = "testuser",
			Password = "test",
			Nickname = "Test User",
			Type = UserType.User,
			StoreId = userStoreId,
			Permissions = new List<UserPermissionEntity>
			{
				new()
				{
					Id = 1,
					UserId = 1,
					Permission = Permission.SpecifikacijaNovcaSviMagacini,
					IsActive = false, // Permission is inactive
				},
			},
		};
		_userRepositoryMock.Setup(r => r.GetCurrentUser()).Returns(currentUser);

		var request = new GetSingleSpecifikacijaNovcaRequest { Id = entityId };

		// Act
		var action = () => _manager.GetSingleAsync(request);

		// Assert
		await action.Should().ThrowAsync<LSCoreForbiddenException>();
	}
}