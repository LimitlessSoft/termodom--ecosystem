using FluentAssertions;
using Moq;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Cities;
using TD.Web.Public.Api.Controllers;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class CitiesControllerTests
{
	private readonly Mock<ICityManager> _managerMock;
	private readonly CitiesController _controller;

	public CitiesControllerTests()
	{
		_managerMock = new Mock<ICityManager>();
		_controller = new CitiesController(_managerMock.Object);
	}

	[Fact]
	public void GetMultiple_ShouldReturnCities()
	{
		// Arrange
		var request = new GetMultipleCitiesRequest();
		var expectedDtos = new List<CityDto> { new() };
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expectedDtos);

		// Act
		var result = _controller.GetMultiple(request);

		// Assert
		result.Should().BeEquivalentTo(expectedDtos);
	}
}
