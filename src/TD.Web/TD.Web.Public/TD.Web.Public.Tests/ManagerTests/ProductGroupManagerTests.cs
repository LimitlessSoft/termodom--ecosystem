using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Requests.ProductsGroups;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class ProductGroupManagerTests : TestBase
{
	private readonly Mock<IProductGroupRepository> _repositoryMock;
	private readonly Mock<ICacheManager> _cacheManagerMock;
	private readonly ProductGroupManager _manager;

	public ProductGroupManagerTests()
	{
		_repositoryMock = new Mock<IProductGroupRepository>();
		_cacheManagerMock = new Mock<ICacheManager>();
		_manager = new ProductGroupManager(_repositoryMock.Object, _cacheManagerMock.Object);
	}

	[Fact]
	public void Get_ValidSrc_ReturnsMappedGroup()
	{
		// Arrange
		var src = "test-src";
		var entities = new List<ProductGroupEntity>
		{
			new()
			{
				Id = 1,
				Src = src,
				IsActive = true,
				Name = "Test",
			},
		};
		_repositoryMock.Setup(r => r.GetMultiple()).Returns(entities.AsQueryable());

		// Act
		var result = _manager.Get(src);

		// Assert
		result.Should().NotBeNull();
		result.Src.Should().Be(src);
	}

	[Fact]
	public void Get_InvalidSrc_ThrowsNotFoundException()
	{
		// Arrange
		_repositoryMock
			.Setup(r => r.GetMultiple())
			.Returns(new List<ProductGroupEntity>().AsQueryable());

		// Act & Assert
		Assert.Throws<LSCore.Exceptions.LSCoreNotFoundException>(() => _manager.Get("invalid"));
	}
}
