using FluentAssertions;
using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Web.Public.Api.Controllers;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Products;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class ProductsControllerTests
{
	private readonly Mock<IProductManager> _managerMock;
	private readonly ProductsController _controller;

	public ProductsControllerTests()
	{
		_managerMock = new Mock<IProductManager>();
		_controller = new ProductsController(_managerMock.Object);
	}

	[Fact]
	public async Task GetMultiple_ShouldReturnProducts()
	{
		// Arrange
		var request = new ProductsGetRequest();
		var expectedResponse = new LSCoreSortedAndPagedResponse<ProductsGetDto>();
		_managerMock.Setup(m => m.GetMultipleAsync(request)).ReturnsAsync(expectedResponse);

		// Act
		var result = await _controller.GetMultiple(request);

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
	}

	[Fact]
	public async Task GetSingle_ShouldReturnProduct()
	{
		// Arrange
		var request = new ProductsGetImageRequest();
		var expectedResponse = new ProductsGetSingleDto();
		_managerMock.Setup(m => m.GetSingleAsync(request)).ReturnsAsync(expectedResponse);

		// Act
		var result = await _controller.GetSingle(request);

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
	}

	[Fact]
	public void AddToCart_ShouldReturnString()
	{
		// Arrange
		var id = 1L;
		var request = new AddToCartRequest();
		var expectedResult = "success";
		_managerMock.Setup(m => m.AddToCart(request)).Returns(expectedResult);

		// Act
		var result = _controller.AddToCart(id, request);

		// Assert
		result.Should().Be(expectedResult);
		request.Id.Should().Be(id);
	}
}
