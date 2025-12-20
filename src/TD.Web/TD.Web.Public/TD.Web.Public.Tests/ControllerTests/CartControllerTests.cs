using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Web.Public.Api.Controllers;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class CartControllerTests
{
	private readonly Mock<ICartManager> _managerMock;
	private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
	private readonly CartController _controller;

	public CartControllerTests()
	{
		_managerMock = new Mock<ICartManager>();
		_httpContextAccessorMock = new Mock<IHttpContextAccessor>();
		_controller = new CartController(_managerMock.Object, _httpContextAccessorMock.Object);
	}

	[Fact]
	public void Get_ShouldReturnCart()
	{
		// Arrange
		var request = new CartGetRequest();
		var expectedDto = new CartGetDto();
		_managerMock.Setup(m => m.Get(request)).Returns(expectedDto);

		// Act
		var result = _controller.Get(request);

		// Assert
		result.Should().Be(expectedDto);
	}

	[Fact]
	public void GetCheckout_ShouldReturnOk()
	{
		// Arrange
		var oneTimeHash = "hash";
		var expectedDto = new CheckoutGetDto();
		_managerMock.Setup(m => m.GetCheckout(oneTimeHash)).Returns(expectedDto);

		// Act
		var result = _controller.GetCheckout(oneTimeHash);

		// Assert
		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expectedDto);
	}

	[Fact]
	public void Checkout_ShouldCallManager()
	{
		// Arrange
		var request = new CheckoutRequestBase();
		_httpContextAccessorMock.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

		// Act
		_controller.Checkout(request);

		// Assert
		_managerMock.Verify(m => m.Checkout(It.IsAny<CheckoutRequest>()), Times.Once);
	}

	[Fact]
	public void GetCurrentLevelInformation_ShouldReturnInfo()
	{
		// Arrange
		var request = new CartCurrentLevelInformationRequest();
		var expectedDto = new CartGetCurrentLevelInformationDto();
		_managerMock.Setup(m => m.GetCurrentLevelInformation(request)).Returns(expectedDto);

		// Act
		var result = _controller.GetCurrentLevelInformation(request);

		// Assert
		result.Should().Be(expectedDto);
	}
}
