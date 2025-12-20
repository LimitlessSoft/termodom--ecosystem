using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Requests.Stavke;
using Xunit;

namespace TD.Komercijalno.Tests.ClientEndpointTests;

public class StavkeEndpointsTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly HttpClient _httpClient;
	private readonly StavkeEndpoints _endpoints;
	private bool _handleStatusCodeCalled;

	public StavkeEndpointsTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		_httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost/"),
		};
		_endpoints = new StavkeEndpoints(() => _httpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task GetMultipleByRobaIdAsync_ShouldReturnStavke()
	{
		// Arrange
		var request = new StavkeGetMultipleByRobaId { RobaId = 1 };
		var expectedResponse = new List<StavkaDto> { new() { RobaId = 1 } };

		_handlerMock
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
					Content = JsonContent.Create(expectedResponse),
				}
			);

		// Act
		var result = await _endpoints.GetMultipleByRobaIdAsync(request);

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task CreateAsync_ShouldReturnCreatedStavka()
	{
		// Arrange
		var request = new StavkaCreateRequest { RobaId = 1 };
		var expectedResponse = new StavkaDto { RobaId = 1 };

		_handlerMock
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
					Content = JsonContent.Create(expectedResponse),
				}
			);

		// Act
		var result = await _endpoints.CreateAsync(request);

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task CreateOptimizedAsync_ShouldReturnStavke()
	{
		// Arrange
		var request = new StavkeCreateOptimizedRequest();
		var expectedResponse = new List<StavkaDto> { new() { RobaId = 1 } };

		_handlerMock
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
					Content = JsonContent.Create(expectedResponse),
				}
			);

		// Act
		var result = await _endpoints.CreateOptimizedAsync(request);

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task DeleteAsync_ShouldCallDelete()
	{
		// Arrange
		var request = new StavkeDeleteRequest
		{
			VrDok = 1,
			BrDok = 1,
			RobaId = 1,
		};

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.Is<HttpRequestMessage>(req =>
					req.Method == HttpMethod.Delete
					&& req.RequestUri!.Query.Contains("VrDok=1")
					&& req.RequestUri.Query.Contains("BrDok=1")
					&& req.RequestUri.Query.Contains("RobaId=1")
				),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

		// Act
		await _endpoints.DeleteAsync(request);

		// Assert
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task GetMultipleByRobaIdAsync_HandleStatusCode_WhenErrorOccurs(
		HttpStatusCode statusCode
	)
	{
		var request = new StavkeGetMultipleByRobaId { RobaId = 1 };
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
			await _endpoints.GetMultipleByRobaIdAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task CreateAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new StavkaCreateRequest { RobaId = 1 };
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
	public async Task CreateOptimizedAsync_HandleStatusCode_WhenErrorOccurs(
		HttpStatusCode statusCode
	)
	{
		var request = new StavkeCreateOptimizedRequest();
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
			await _endpoints.CreateOptimizedAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task DeleteAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new StavkeDeleteRequest();
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
			await _endpoints.DeleteAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}
}
