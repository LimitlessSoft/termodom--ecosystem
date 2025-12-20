using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LSCore.Common.Contracts;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Requests.Roba;
using Xunit;

namespace TD.Komercijalno.Tests.ClientEndpointTests;

public class RobaEndpointsTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly RobaEndpoints _endpoints;
	private bool _handleStatusCodeCalled;

	public RobaEndpointsTests()
	{
		// Reset static cache before each test
		var cacheField = typeof(RobaEndpoints).GetField(
			"__getMultipleAsyncCacheData",
			System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic
		);
		cacheField?.SetValue(null, null);
		var timeField = typeof(RobaEndpoints).GetField(
			"__getMultipleAsyncCacheTime",
			System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic
		);
		timeField?.SetValue(null, null);

		_handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost"),
		};
		_endpoints = new RobaEndpoints(() => httpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task Get_ReturnsRobaDto()
	{
		var request = new LSCoreIdRequest { Id = 1 };
		var expected = new RobaDto();
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

		var result = await _endpoints.Get(request);

		result.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task GetMultipleAsync_ReturnsList_AndUsesCache()
	{
		var request = new RobaGetMultipleRequest();
		var expected = new List<RobaDto> { new() };
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

		// First call - should hit the network
		var result1 = await _endpoints.GetMultipleAsync(request);
		result1.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();

		_handleStatusCodeCalled = false;

		// Second call - should hit the cache
		var result2 = await _endpoints.GetMultipleAsync(request);
		result2.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeFalse(); // Cache hit, so handleStatusCode not called again

		_handlerMock
			.Protected()
			.Verify(
				"SendAsync",
				Times.Once(),
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			);
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task Get_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new LSCoreIdRequest { Id = 1 };
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
			await _endpoints.Get(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task GetMultipleAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new RobaGetMultipleRequest { Vrsta = (short)statusCode };
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
