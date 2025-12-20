using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using Xunit;

namespace TD.Komercijalno.Tests.ClientEndpointTests;

public class VrstaDokEndpointsTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly VrstaDokEndpoints _endpoints;
	private bool _handleStatusCodeCalled;

	public VrstaDokEndpointsTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost"),
		};
		_endpoints = new VrstaDokEndpoints(() => httpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task GetMultiple_ReturnsList()
	{
		var expected = new List<VrstaDokDto> { new() };
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

		var result = await _endpoints.GetMultiple();

		result.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task GetMultiple_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(new HttpResponseMessage { StatusCode = statusCode });

		try
		{
			await _endpoints.GetMultiple();
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}
}
