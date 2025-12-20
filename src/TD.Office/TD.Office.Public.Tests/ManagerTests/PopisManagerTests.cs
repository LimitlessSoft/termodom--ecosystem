using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Public.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Popisi;
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

		_configurationRootMock.Setup(c => c["DEPLOY_ENV"]).Returns("develop");

		// Act & Assert
		// We expect it to fail here because we didn't mock client.Dokumenti.Get
		// but at least we've set up the foundations.
		var action = () => _manager.GetById(id);
		action.Should().Throw<Exception>();
	}

	[Fact]
	public async Task MasovnoDodavanjeStavkiAsync_ValidatesMistakeInLastCommit()
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
			KomercijalnoPopisBrDok = 123,
			Items = new List<PopisItemEntity>(),
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

		var handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost/"),
		};

		var dokumentiResponse = new List<DokumentDto>
		{
			new()
			{
				Stavke = new List<StavkaDto>
				{
					new() { RobaId = 101, Kolicina = 10 },
					new() { RobaId = 102, Kolicina = 20 },
				},
			},
		};

		handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("dokumenti")
				),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(
				new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = JsonContent.Create(dokumentiResponse),
				}
			);

		handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Post
					&& req.RequestUri.ToString().Contains("stavke-optimized")
				),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(
				new HttpResponseMessage
				{
					StatusCode = HttpStatusCode.OK,
					Content = JsonContent.Create(new List<StavkaDto>()),
				}
			);

		var dokumentiEndpoints = new DokumentiEndpoints(() => httpClient, _ => { });
		var stavkeEndpoints = new StavkeEndpoints(() => httpClient, _ => { });

		typeof(TDKomercijalnoClient)
			.GetProperty("Dokumenti")
			.SetValue(_komercijalnoClientMock.Object, dokumentiEndpoints);
		typeof(TDKomercijalnoClient)
			.GetProperty("Stavke")
			.SetValue(_komercijalnoClientMock.Object, stavkeEndpoints);

		_komercijalnoClientFactoryMock
			.Setup(f =>
				f.Create(
					It.IsAny<int>(),
					It.IsAny<TDKomercijalnoEnvironment>(),
					It.IsAny<TDKomercijalnoFirma>()
				)
			)
			.Returns(_komercijalnoClientMock.Object);

		_configurationRootMock.Setup(c => c["DEPLOY_ENV"]).Returns("develop");

		var request = new PopisMasovnoDodavanjeStavkiRequest
		{
			ActionType = PopisMasovnoDodavanjeStavkiActionType.StavkePocetnogStanajSaKolicinom,
		};

		// Act
		await _manager.MasovnoDodavanjeStavkiAsync(id, request);

		// Assert
		// 1. Check that PopisanaKolicina is 99999 (the mistake in the last commit)
		entity.Items.Should().HaveCount(2);
		entity.Items.All(x => x.PopisanaKolicina == 99999).Should().BeTrue();

		// 2. Check that Kolicina in external request was 0 (the other part of the change)
		handlerMock
			.Protected()
			.Verify(
				"SendAsync",
				Times.AtLeastOnce(),
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Post
					&& req.RequestUri.ToString().Contains("stavke-optimized")
					&& req.Content.ReadAsStringAsync().Result.Contains("0")
				),
				ItExpr.IsAny<CancellationToken>()
			);
	}
}
