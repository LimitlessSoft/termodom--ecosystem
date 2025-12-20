using FluentAssertions;
using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Web.Public.Api.Controllers;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Orders;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class OrdersControllerTests
{
	private readonly Mock<IOrderManager> _managerMock;
	private readonly OrdersController _controller;

	public OrdersControllerTests()
	{
		_managerMock = new Mock<IOrderManager>();
		_controller = new OrdersController(_managerMock.Object);
	}

	[Fact]
	public void GetMultiple_ShouldReturnOrders()
	{
		// Arrange
		var request = new GetMultipleOrdersRequest();
		var expectedResponse = new LSCoreSortedAndPagedResponse<OrdersGetDto>();
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expectedResponse);

		// Act
		var result = _controller.GetMultiple(request);

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
	}

	[Fact]
	public void GetOrdersInfo_ShouldReturnOrdersInfo()
	{
		// Arrange
		var expectedResponse = new OrdersInfoDto();
		_managerMock.Setup(m => m.GetOrdersInfo()).Returns(expectedResponse);

		// Act
		var result = _controller.GetOrdersInfo();

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
	}

	[Fact]
	public void GetSingle_ShouldReturnOrder()
	{
		// Arrange
		var request = new GetSingleOrderRequest { OneTimeHash = "hash" };
		var expectedResponse = new OrderGetSingleDto
		{
			Status = "Pending",
			StatusId = 1,
			Items = new List<OrdersItemDto>(),
		};
		_managerMock.Setup(m => m.GetSingle(request)).Returns(expectedResponse);

		// Act
		var result = _controller.GetSingle(request);

		// Assert
		result.Should().BeEquivalentTo(expectedResponse);
	}
}
