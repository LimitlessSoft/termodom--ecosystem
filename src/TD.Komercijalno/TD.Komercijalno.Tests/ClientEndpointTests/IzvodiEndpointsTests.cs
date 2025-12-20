using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using Xunit;

namespace TD.Komercijalno.Tests.ClientEndpointTests;

public class IzvodiEndpointsTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly IzvodiEndpoints _endpoints;
	private bool _handleStatusCodeCalled;

	public IzvodiEndpointsTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost"),
		};
		_endpoints = new IzvodiEndpoints(() => httpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task GetMultipleAsync_ReturnsList()
	{
		var request = new IzvodGetMultipleRequest();
		var expected = new List<IzvodDto> { new() };
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

		var result = await _endpoints.GetMultipleAsync(request);

		result.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task GetMultipleAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new IzvodGetMultipleRequest();
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
			await _endpoints.GetMultipleAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}
}
