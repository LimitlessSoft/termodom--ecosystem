using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;
using Xunit;

namespace TD.Komercijalno.Tests.ClientEndpointTests;

public class DokumentiEndpointsTests : EndpointTestBase
{
	private readonly DokumentiEndpoints _endpoints;

	public DokumentiEndpointsTests()
	{
		_endpoints = new DokumentiEndpoints(CreateHttpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task Get_ReturnsDokumentDto()
	{
		var request = new DokumentGetRequest { VrDok = 1, BrDok = 1 };
		var expected = new DokumentDto();
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

		var result = await _endpoints.GetAsync(request);

		result.Should().BeEquivalentTo(expected);
		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task GetMultipleAsync_ReturnsList()
	{
		var request = new DokumentGetMultipleRequest();
		var expected = new List<DokumentDto> { new() };
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

	[Fact]
	public async Task CreateAsync_ReturnsDokumentDto()
	{
		var request = new DokumentCreateRequest();
		var expected = new DokumentDto();
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
	public async Task UpdateDokOut_CallsHandleStatusCode()
	{
		var request = new DokumentSetDokOutRequest { VrDok = 1, BrDok = 1 };
		var response = new HttpResponseMessage(HttpStatusCode.OK);

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(response);

		await _endpoints.UpdateDokOut(request);

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Fact]
	public async Task SetDokumenFlag_CallsHandleStatusCode()
	{
		var request = new DokumentSetFlagRequest
		{
			VrDok = 1,
			BrDok = 1,
			Flag = 1,
		};
		var response = new HttpResponseMessage(HttpStatusCode.OK);

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(response);

		await _endpoints.SetDokumenFlag(request);

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task Get_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new DokumentGetRequest { VrDok = 1, BrDok = 1 };
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
			await _endpoints.GetAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task GetMultipleAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new DokumentGetMultipleRequest();
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

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task CreateAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new DokumentCreateRequest();
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
	public async Task UpdateDokOut_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new DokumentSetDokOutRequest { VrDok = 1, BrDok = 1 };
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
			await _endpoints.UpdateDokOut(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task SetDokumenFlag_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new DokumentSetFlagRequest
		{
			VrDok = 1,
			BrDok = 1,
			Flag = 1,
		};
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
			await _endpoints.SetDokumenFlag(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}
}
