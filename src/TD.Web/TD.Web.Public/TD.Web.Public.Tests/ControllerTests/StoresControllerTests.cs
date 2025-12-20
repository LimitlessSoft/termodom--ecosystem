using FluentAssertions;
using Moq;
using TD.Web.Common.Contracts.Dtos.Stores;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Stores;
using TD.Web.Public.Api.Controllers;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class StoresControllerTests
{
	private readonly Mock<IStoreManager> _managerMock;
	private readonly StoresController _controller;

	public StoresControllerTests()
	{
		_managerMock = new Mock<IStoreManager>();
		_controller = new StoresController(_managerMock.Object);
	}

	[Fact]
	public void GetMultiple_ShouldReturnStores()
	{
		// Arrange
		var request = new GetMultipleStoresRequest();
		var expectedDtos = new List<StoreDto> { new() };
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expectedDtos);

		// Act
		var result = _controller.GetMultiple(request);

		// Assert
		result.Should().BeEquivalentTo(expectedDtos);
	}
}
