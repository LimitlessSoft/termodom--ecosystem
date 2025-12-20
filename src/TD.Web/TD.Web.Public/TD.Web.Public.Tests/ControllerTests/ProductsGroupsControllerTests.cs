using FluentAssertions;
using Moq;
using TD.Web.Public.Api.Controllers;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.ProductsGroups;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class ProductsGroupsControllerTests
{
	private readonly Mock<IProductGroupManager> _managerMock;
	private readonly ProductsGroupsController _controller;

	public ProductsGroupsControllerTests()
	{
		_managerMock = new Mock<IProductGroupManager>();
		_controller = new ProductsGroupsController(_managerMock.Object);
	}

	[Fact]
	public void Get_ShouldReturnProductGroup()
	{
		// Arrange
		var src = "test-src";
		var expectedDto = new ProductsGroupsGetDto();
		_managerMock.Setup(m => m.Get(src)).Returns(expectedDto);

		// Act
		var result = _controller.Get(src);

		// Assert
		result.Should().Be(expectedDto);
	}

	[Fact]
	public void GetMultiple_ShouldReturnProductGroups()
	{
		// Arrange
		var request = new ProductsGroupsGetRequest();
		var expectedDtos = new List<ProductsGroupsGetDto> { new() };
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expectedDtos);

		// Act
		var result = _controller.GetMultiple(request);

		// Assert
		result.Should().BeEquivalentTo(expectedDtos);
	}
}
