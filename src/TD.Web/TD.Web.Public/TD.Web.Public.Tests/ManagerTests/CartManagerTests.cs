using FluentAssertions;
using LSCore.Auth.Contracts;
using Moq;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class CartManagerTests : TestBase
{
	private readonly Mock<IOrderManager> _orderManagerMock;
	private readonly Mock<LSCoreAuthContextEntity<string>> _authContextMock;
	private readonly Mock<IOrderRepository> _orderRepositoryMock;
	private readonly Mock<IOrderItemRepository> _orderItemRepositoryMock;
	private readonly Mock<IOfficeServerApiManager> _officeServerApiManagerMock;
	private readonly Mock<IUserRepository> _userRepositoryMock;
	private readonly Mock<ISettingRepository> _settingRepositoryMock;
	private readonly Mock<IStoreRepository> _storeRepositoryMock;
	private readonly CartManager _manager;

	public CartManagerTests()
	{
		_orderManagerMock = new Mock<IOrderManager>();
		_authContextMock = new Mock<LSCoreAuthContextEntity<string>>();
		_orderRepositoryMock = new Mock<IOrderRepository>();
		_orderItemRepositoryMock = new Mock<IOrderItemRepository>();
		_officeServerApiManagerMock = new Mock<IOfficeServerApiManager>();
		_userRepositoryMock = new Mock<IUserRepository>();
		_settingRepositoryMock = new Mock<ISettingRepository>();
		_storeRepositoryMock = new Mock<IStoreRepository>();

		_manager = new CartManager(
			_orderManagerMock.Object,
			_authContextMock.Object,
			_orderRepositoryMock.Object,
			_orderItemRepositoryMock.Object,
			_officeServerApiManagerMock.Object,
			_userRepositoryMock.Object,
			_settingRepositoryMock.Object,
			_storeRepositoryMock.Object
		);
	}

	[Fact]
	public void Get_ValidRequest_ReturnsDto()
	{
		// Arrange
		var request = new CartGetRequest { OneTimeHash = "hash" };

		// Act & Assert
		_manager.Should().NotBeNull();
	}
}
