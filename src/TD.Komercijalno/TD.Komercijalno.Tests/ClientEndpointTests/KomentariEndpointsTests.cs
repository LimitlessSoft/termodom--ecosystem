using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Requests.Komentari;
using Xunit;

namespace TD.Komercijalno.Tests.ClientEndpointTests;

public class KomentariEndpointsTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly KomentariEndpoints _endpoints;
	private bool _handleStatusCodeCalled;

	public KomentariEndpointsTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost"),
		};
		_endpoints = new KomentariEndpoints(() => httpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task CreateAsync_ReturnsKomentarDto()
	{
		var request = new CreateKomentarRequest();
		var expected = new KomentarDto();
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

		var result = await _endpoints.CreateAsync(request);

		result.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task Flush_CallsHandleStatusCode()
	{
		var request = new FlushCommentsRequest();
		var response = new HttpResponseMessage(HttpStatusCode.OK);

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(response);

		await _endpoints.Flush(request);

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task CreateAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new CreateKomentarRequest();
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
			await _endpoints.CreateAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task Flush_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new FlushCommentsRequest();
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
			await _endpoints.Flush(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}
}
