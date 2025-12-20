using Moq;

namespace TD.Office.Public.Tests;

public abstract class EndpointTestBase
{
	protected readonly Mock<HttpMessageHandler> _handlerMock = new();
	protected bool _handleStatusCodeCalled;

	protected HttpClient CreateHttpClient()
	{
		return new HttpClient(_handlerMock.Object)
		{
			BaseAddress = new Uri("http://localhost"),
		};
	}
}
