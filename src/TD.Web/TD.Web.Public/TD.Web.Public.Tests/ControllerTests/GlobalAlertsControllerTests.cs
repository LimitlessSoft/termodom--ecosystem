using FluentAssertions;
using Moq;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.GlobalAlerts;
using TD.Web.Public.Api.Controllers;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class GlobalAlertsControllerTests
{
	private readonly Mock<IGlobalAlertManager> _managerMock;
	private readonly GlobalAlertsController _controller;

	public GlobalAlertsControllerTests()
	{
		_managerMock = new Mock<IGlobalAlertManager>();
		_controller = new GlobalAlertsController(_managerMock.Object);
	}

	[Fact]
	public void GetMultiple_ShouldReturnAlerts()
	{
		// Arrange
		var request = new GlobalAlertsGetMultipleRequest();
		var expectedDtos = new List<GlobalAlertDto> { new() };
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expectedDtos);

		// Act
		var result = _controller.GetMultiple(request);

		// Assert
		result.Should().BeEquivalentTo(expectedDtos);
	}
}
