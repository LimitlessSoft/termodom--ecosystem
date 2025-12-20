using FluentAssertions;
using LSCore.Auth.Contracts;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class ProductManagerTests : TestBase
{
	private readonly Mock<ILogger<ProductManager>> _loggerMock;
	private readonly Mock<IProductGroupRepository> _productGroupRepositoryMock;
	private readonly Mock<IOrderRepository> _orderRepositoryMock;
	private readonly Mock<IOrderManager> _orderManagerMock;
	private readonly Mock<IImageManager> _imageManagerMock;
	private readonly Mock<IMemoryCache> _memoryCacheMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly Mock<IProductPriceGroupLevelRepository> _productPriceGroupLevelRepositoryMock;
	private readonly Mock<LSCoreAuthContextEntity<string>> _contextEntityMock;
	private readonly Mock<IProductRepository> _productRepositoryMock;
	private readonly Mock<IOrderItemRepository> _orderItemRepositoryMock;
	private readonly Mock<ICacheManager> _cacheManagerMock;
	private readonly ProductManager _manager;

	public ProductManagerTests()
	{
		_loggerMock = new Mock<ILogger<ProductManager>>();
		_productGroupRepositoryMock = new Mock<IProductGroupRepository>();
		_orderRepositoryMock = new Mock<IOrderRepository>();
		_orderManagerMock = new Mock<IOrderManager>();
		_imageManagerMock = new Mock<IImageManager>();
		_memoryCacheMock = new Mock<IMemoryCache>();
		_userRepositoryMock = new Mock<IUserRepository>();
		_productPriceGroupLevelRepositoryMock = new Mock<IProductPriceGroupLevelRepository>();
		_contextEntityMock = new Mock<LSCoreAuthContextEntity<string>>();
		_productRepositoryMock = new Mock<IProductRepository>();
		_orderItemRepositoryMock = new Mock<IOrderItemRepository>();
		_cacheManagerMock = new Mock<ICacheManager>();

		_manager = new ProductManager(
			_loggerMock.Object,
			_productGroupRepositoryMock.Object,
			_orderRepositoryMock.Object,
			_orderManagerMock.Object,
			_imageManagerMock.Object,
			_memoryCacheMock.Object,
			_userRepositoryMock.Object,
			_productPriceGroupLevelRepositoryMock.Object,
			_contextEntityMock.Object,
			_productRepositoryMock.Object,
			_orderItemRepositoryMock.Object,
			_cacheManagerMock.Object
		);
	}

	// [Fact]
	// public void AddToCart_ValidRequest_CallsOrderManager()
	// {
	// 	// Arrange
	// 	var productId = 12345L;
	// 	var request = new AddToCartRequest
	// 	{
	// 		Id = productId,
	// 		Quantity = 1,
	// 		OneTimeHash = "hash",
	// 	};
	// 	_dbContext.Products.Add(
	// 		new ProductEntity
	// 		{
	// 			Id = productId,
	// 			IsActive = true,
	// 			Name = "Test",
	// 			Image = "test.png",
	// 			Src = "test",
	// 			OneAlternatePackageEquals = 1,
	// 			UnitId = 1,
	// 			PriceId = 1,
	// 			ProductPriceGroupId = 1,
	// 			Status = ProductStatus.Vidljiv,
	// 		}
	// 	);
	// 	_dbContext.SaveChanges();
	//
	// 	// Verify it's in DB
	// 	_dbContext.Products.Any(x => x.Id == productId).Should().BeTrue();
	//
	// 	_orderManagerMock.Setup(m => m.AddItem(It.IsAny<OrdersAddItemRequest>())).Returns("hash");
	//
	// 	// Act
	// 	var result = _manager.AddToCart(request);
	//
	// 	// Assert
	// 	result.Should().Be("hash");
	// 	_orderManagerMock.Verify(m => m.AddItem(It.IsAny<OrdersAddItemRequest>()), Times.Once);
	// }
}
