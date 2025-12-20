using FluentAssertions;
using Moq;
using TD.Web.Public.Api.Controllers;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class PaymentTypesControllerTests
{
	private readonly Mock<IPaymentTypeManager> _managerMock;
	private readonly PaymentTypesController _controller;

	public PaymentTypesControllerTests()
	{
		_managerMock = new Mock<IPaymentTypeManager>();
		_controller = new PaymentTypesController(_managerMock.Object);
	}

	[Fact]
	public void GetMultiple_ShouldReturnPaymentTypes()
	{
		// Arrange
		var expectedDtos = new List<PaymentTypeGetDto> { new() };
		_managerMock.Setup(m => m.GetMultiple()).Returns(expectedDtos);

		// Act
		var result = _controller.GetMultiple();

		// Assert
		result.Should().BeEquivalentTo(expectedDtos);
	}
}
