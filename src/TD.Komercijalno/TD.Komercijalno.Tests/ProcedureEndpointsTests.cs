using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Requests.Procedure;
using Xunit;

namespace TD.Komercijalno.Tests;

public class ProcedureEndpointsTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly ProcedureEndpoints _endpoints;
	private bool _handleStatusCodeCalled;

	public ProcedureEndpointsTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost"),
		};
		_endpoints = new ProcedureEndpoints(() => httpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task GetProdajnaCenaNaDanAsync_ReturnsDecimal()
	{
		var request = new ProceduraGetProdajnaCenaNaDanRequest { Datum = DateTime.Now };
		var response = new HttpResponseMessage(HttpStatusCode.OK)
		{
			Content = JsonContent.Create(10.5m),
		};

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(response);

		var result = await _endpoints.GetProdajnaCenaNaDanAsync(request);

		result.Should().Be(10.5m);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task GetProdajnaCenaNaDanOptimizedAsync_ReturnsList()
	{
		var request = new ProceduraGetProdajnaCenaNaDanOptimizedRequest { Datum = DateTime.Now };
		var expected = new List<ProdajnaCenaNaDanDto> { new() };
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

		var result = await _endpoints.GetProdajnaCenaNaDanOptimizedAsync(request);

		result.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task GetProdajnaCenaNaDanAsync_HandleStatusCode_WhenErrorOccurs(
		HttpStatusCode statusCode
	)
	{
		var request = new ProceduraGetProdajnaCenaNaDanRequest { Datum = DateTime.Now };
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
			await _endpoints.GetProdajnaCenaNaDanAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task GetProdajnaCenaNaDanOptimizedAsync_HandleStatusCode_WhenErrorOccurs(
		HttpStatusCode statusCode
	)
	{
		var request = new ProceduraGetProdajnaCenaNaDanOptimizedRequest { Datum = DateTime.Now };
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
			await _endpoints.GetProdajnaCenaNaDanOptimizedAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}
}
