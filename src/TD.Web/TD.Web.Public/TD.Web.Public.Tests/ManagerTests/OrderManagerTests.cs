using FluentAssertions;
using LSCore.Auth.Contracts;
using Moq;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Public.Contracts.Requests.Orders;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class OrderManagerTests : TestBase
{
	private readonly Mock<IOrderItemManager> _orderItemManagerMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly Mock<IOrderRepository> _orderRepositoryMock;
	private readonly Mock<IProductRepository> _productRepositoryMock;
	private readonly Mock<IPaymentTypeRepository> _paymentTypeRepositoryMock;
	private readonly Mock<LSCoreAuthContextEntity<string>> _contextEntityMock;
	private readonly OrderManager _manager;

	public OrderManagerTests()
	{
		_orderItemManagerMock = new Mock<IOrderItemManager>();
		_userRepositoryMock = new Mock<IUserRepository>();
		_orderRepositoryMock = new Mock<IOrderRepository>();
		_productRepositoryMock = new Mock<IProductRepository>();
		_paymentTypeRepositoryMock = new Mock<IPaymentTypeRepository>();
		_contextEntityMock = new Mock<LSCoreAuthContextEntity<string>>();

		_manager = new OrderManager(
			_orderItemManagerMock.Object,
			_userRepositoryMock.Object,
			_orderRepositoryMock.Object,
			_productRepositoryMock.Object,
			_paymentTypeRepositoryMock.Object,
			_contextEntityMock.Object
		);
	}

	[Fact]
	public void AddItem_ProductNotFound_ThrowsNotFoundException()
	{
		// Arrange
		var request = new OrdersAddItemRequest { ProductId = 1 };
		_productRepositoryMock
			.Setup(r => r.GetMultiple())
			.Returns(new List<ProductEntity>().AsQueryable());

		// Act & Assert
		Assert.Throws<LSCore.Exceptions.LSCoreNotFoundException>(() => _manager.AddItem(request));
	}

	[Fact]
	public void AddItem_ValidRequest_ReturnsOneTimeHash()
	{
		// Arrange
		var productId = 1L;
		var request = new OrdersAddItemRequest
		{
			ProductId = productId,
			Quantity = 1,
			OneTimeHash = "hash",
		};
		var product = new ProductEntity { Id = productId, Name = "Test" };
		var order = new OrderEntity
		{
			Id = 1,
			OneTimeHash = "hash",
			Status = OrderStatus.Open,
		};

		_productRepositoryMock
			.Setup(r => r.GetMultiple())
			.Returns(new List<ProductEntity> { product }.AsQueryable());
		_orderRepositoryMock
			.Setup(r => r.GetMultiple())
			.Returns(new List<OrderEntity> { order }.AsQueryable());
		_orderItemManagerMock
			.Setup(m => m.Exists(It.IsAny<OrderItemExistsRequest>()))
			.Returns(false);
		_paymentTypeRepositoryMock
			.Setup(r => r.GetMultiple())
			.Returns(
				new List<PaymentTypeEntity>
				{
					new() { Id = 1, IsActive = true },
				}.AsQueryable()
			);

		// Act
		var result = _manager.AddItem(request);

		// Assert
		result.Should().Be("hash");
		_orderItemManagerMock.Verify(m => m.Insert(It.IsAny<OrderItemEntity>()), Times.Once);
	}
}
