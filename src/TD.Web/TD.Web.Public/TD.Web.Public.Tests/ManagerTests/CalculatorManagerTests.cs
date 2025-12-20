using FluentAssertions;
using Moq;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts.Dtos.Calculator;
using TD.Web.Public.Contracts.Requests.Calculator;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class CalculatorManagerTests : TestBase
{
	private readonly Mock<ICalculatorItemRepository> _calculatorItemRepositoryMock;
	private readonly CalculatorManager _manager;

	public CalculatorManagerTests()
	{
		_calculatorItemRepositoryMock = new Mock<ICalculatorItemRepository>();
		_manager = new CalculatorManager(_calculatorItemRepositoryMock.Object);
	}

	[Fact]
	public void GetCalculatorItems_ValidRequest_ReturnsDtos()
	{
		// Arrange
		var type = CalculatorType.Fasada;
		var request = new GetCalculatorItemsRequest { Type = type };
		var items = new List<CalculatorItemEntity>
		{
			new()
			{
				CalculatorType = type,
				Quantity = 1,
				Product = new ProductEntity
				{
					Name = "Product 1",
					Unit = new UnitEntity { Name = "kom" },
				},
				IsPrimary = true,
			},
		};

		_calculatorItemRepositoryMock.Setup(r => r.GetMultiple()).Returns(items.AsQueryable());

		// Act
		var result = _manager.GetCalculatorItems(request);

		// Assert
		result.Should().HaveCount(1);
		result[0].ProductName.Should().Be("Product 1");
		result[0].Unit.Should().Be("kom");
		result[0].Quantity.Should().Be(1);
		result[0].IsPrimary.Should().BeTrue();
	}

	[Fact]
	public void GetCalculator_ValidRequest_ReturnsDto()
	{
		// Arrange
		var type = CalculatorType.Fasada;
		var request = new GetCalculatorRequest { Type = type, Quantity = 1 };
		var items = new List<CalculatorItemEntity>
		{
			new()
			{
				CalculatorType = type,
				Quantity = 1,
				IsStandard = true,
				Product = new ProductEntity
				{
					Price = new ProductPriceEntity { Min = 100, Max = 120 },
					Unit = new UnitEntity { Name = "kom" },
				},
			},
		};

		_calculatorItemRepositoryMock.Setup(r => r.GetMultiple()).Returns(items.AsQueryable());

		// Act
		var result = _manager.GetCalculator(request);

		// Assert
		result.Should().NotBeNull();
		// In CalculatorManager.cs, only StandardValueWithVAT is currently implemented and it uses PricesHelpers.CalculateOneTimeCartPrice
		result.StandardValueWithVAT.Should().BeGreaterThan(0);
	}
}
