using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Office.Public.Client.Endpoints;
using TD.Office.Public.Contracts.Dtos.KomercijalnoMagacinFirma;
using Xunit;

namespace TD.Office.Public.Tests.EndpointTests;

public class KomercijalnoMagacinFirmaEndpointsTests : EndpointTestBase
{
	private readonly KomercijalnoMagacinFirmaEndpoints _endpoints;

	public KomercijalnoMagacinFirmaEndpointsTests()
	{
		_endpoints = new KomercijalnoMagacinFirmaEndpoints(
			CreateHttpClient,
			_ => _handleStatusCodeCalled = true
		);
	}

	[Fact]
	public async Task Get_ReturnsDto()
	{
		// Arrange
		var magacinId = 1;
		var expected = new GetKomercijalnoMagacinFirmaDto { MagacinId = magacinId };
		var response = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = JsonContent.Create(expected),
		};

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(response);

		// Act
		var result = await _endpoints.Get(magacinId);

		// Assert
		result.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task Get_WhenNotFound_ThrowsLSCoreBadRequestException()
	{
		// Arrange
		var magacinId = 1;
		var response = new HttpResponseMessage(HttpStatusCode.NotFound);

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(response);

		// Act
		var action = () => _endpoints.Get(magacinId);

		// Assert
		await action.Should().ThrowAsync<LSCore.Exceptions.LSCoreBadRequestException>();
	}
}
