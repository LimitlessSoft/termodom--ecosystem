using System.Net;
using FluentAssertions;
using Moq;
using Moq.Protected;
using TD.Komercijalno.Client.Endpoints;
using TD.Komercijalno.Contracts.Requests.Parametri;
using Xunit;

namespace TD.Komercijalno.Tests;

public class ParametriEndpointsTests
{
	private readonly Mock<HttpMessageHandler> _handlerMock;
	private readonly ParametriEndpoints _endpoints;
	private bool _handleStatusCodeCalled;

	public ParametriEndpointsTests()
	{
		_handlerMock = new Mock<HttpMessageHandler>();
		var httpClient = new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost"),
		};
		_endpoints = new ParametriEndpoints(() => httpClient, _ => _handleStatusCodeCalled = true);
	}

	[Fact]
	public async Task UpdateAsync_CallsHandleStatusCode()
	{
		var request = new UpdateParametarRequest();
		var response = new HttpResponseMessage(HttpStatusCode.OK);

		_handlerMock
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>()
			)
			.ReturnsAsync(response);

		await _endpoints.UpdateAsync(request);

		_handleStatusCodeCalled.Should().BeTrue();
	}

	[Theory]
	[InlineData(HttpStatusCode.BadRequest)]
	[InlineData(HttpStatusCode.NotFound)]
	public async Task UpdateAsync_HandleStatusCode_WhenErrorOccurs(HttpStatusCode statusCode)
	{
		var request = new UpdateParametarRequest();
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
			await _endpoints.UpdateAsync(request);
		}
		catch { }

		_handleStatusCodeCalled.Should().BeTrue();
	}
}
