using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Domain.Validators.Products;
using Xunit;

namespace TD.Web.Public.Tests.ValidatorTests;

public class AddToCartRequestValidatorTests
{
	private readonly WebDbContext _dbContext;
	private readonly AddToCartRequestValidator _validator;

	public AddToCartRequestValidatorTests()
	{
		var options = new DbContextOptionsBuilder<WebDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

		var configurationMock = new Mock<IConfigurationRoot>();
		configurationMock.Setup(c => c["POSTGRES_HOST"]).Returns("localhost");
		configurationMock.Setup(c => c["POSTGRES_PORT"]).Returns("5432");
		configurationMock.Setup(c => c["POSTGRES_USER"]).Returns("user");
		configurationMock.Setup(c => c["POSTGRES_PASSWORD"]).Returns("pass");
		configurationMock.Setup(c => c["DEPLOY_ENV"]).Returns("test");

		_dbContext = new TestWebDbContext(options, configurationMock.Object);

		var dbContextFactoryMock = new Mock<IWebDbContextFactory>();
		dbContextFactoryMock.Setup(f => f.Create<WebDbContext>()).Returns(_dbContext);

		_validator = new AddToCartRequestValidator(dbContextFactoryMock.Object);
	}

	[Fact]
	public void Validate_ProductNotFound_ReturnsError()
	{
		// Arrange
		var request = new AddToCartRequest
		{
			Id = 999,
			Quantity = 1
		};

		// Act
		var result = _validator.TestValidate(request);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public void Validate_ProductInactive_ReturnsError()
	{
		// Arrange
		var product = CreateProduct(id: 1, isActive: false);
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		var request = new AddToCartRequest
		{
			Id = 1,
			Quantity = 1
		};

		// Act
		var result = _validator.TestValidate(request);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public void Validate_QuantityZero_ReturnsError()
	{
		// Arrange
		var product = CreateProduct(id: 2);
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		var request = new AddToCartRequest
		{
			Id = 2,
			Quantity = 0
		};

		// Act
		var result = _validator.TestValidate(request);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public void Validate_OneAlternatePackageEqualsZero_DoesNotThrowDivideByZero()
	{
		// Arrange - This is the bug scenario: product with OneAlternatePackageEquals = 0
		var product = CreateProduct(id: 3, oneAlternatePackageEquals: 0);
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		var request = new AddToCartRequest
		{
			Id = 3,
			Quantity = 5
		};

		// Act & Assert - Should not throw DivideByZeroException
		var act = () => _validator.TestValidate(request);
		act.Should().NotThrow<DivideByZeroException>();

		// Validation should pass since OneAlternatePackageEquals = 0 is treated as "no constraint"
		var result = _validator.TestValidate(request);
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Validate_OneAlternatePackageEqualsNull_ValidQuantity_Passes()
	{
		// Arrange
		var product = CreateProduct(id: 4, oneAlternatePackageEquals: null);
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		var request = new AddToCartRequest
		{
			Id = 4,
			Quantity = 7
		};

		// Act
		var result = _validator.TestValidate(request);

		// Assert
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Validate_QuantityNotDivisibleByPackageSize_ReturnsError()
	{
		// Arrange - Product must be ordered in multiples of 5
		var product = CreateProduct(id: 5, oneAlternatePackageEquals: 5);
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		var request = new AddToCartRequest
		{
			Id = 5,
			Quantity = 7 // Not divisible by 5
		};

		// Act
		var result = _validator.TestValidate(request);

		// Assert
		result.IsValid.Should().BeFalse();
	}

	[Fact]
	public void Validate_QuantityDivisibleByPackageSize_Passes()
	{
		// Arrange - Product must be ordered in multiples of 5
		var product = CreateProduct(id: 6, oneAlternatePackageEquals: 5);
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		var request = new AddToCartRequest
		{
			Id = 6,
			Quantity = 10 // Divisible by 5
		};

		// Act
		var result = _validator.TestValidate(request);

		// Assert
		result.IsValid.Should().BeTrue();
	}

	[Fact]
	public void Validate_ValidRequest_Passes()
	{
		// Arrange
		var product = CreateProduct(id: 7);
		_dbContext.Products.Add(product);
		_dbContext.SaveChanges();

		var request = new AddToCartRequest
		{
			Id = 7,
			Quantity = 1
		};

		// Act
		var result = _validator.TestValidate(request);

		// Assert
		result.IsValid.Should().BeTrue();
	}

	private static ProductEntity CreateProduct(
		long id,
		bool isActive = true,
		decimal? oneAlternatePackageEquals = null
	)
	{
		return new ProductEntity
		{
			Id = id,
			IsActive = isActive,
			Name = $"Test Product {id}",
			Image = "test.png",
			Src = $"test-product-{id}",
			OneAlternatePackageEquals = oneAlternatePackageEquals,
			UnitId = 1,
			PriceId = 1,
			ProductPriceGroupId = 1,
			Status = ProductStatus.Vidljiv,
		};
	}
}
