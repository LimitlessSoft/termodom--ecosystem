using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Web.Public.Api.Controllers;
using TD.Web.Public.Contracts.Dtos.Calculator;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Calculator;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class CalculatorControllerTests
{
	private readonly Mock<ICalculatorManager> _managerMock;
	private readonly CalculatorController _controller;

	public CalculatorControllerTests()
	{
		_managerMock = new Mock<ICalculatorManager>();
		_controller = new CalculatorController(_managerMock.Object);
	}

	[Fact]
	public void GetCalculatorItems_ShouldReturnOkWithItems()
	{
		// Arrange
		var request = new GetCalculatorItemsRequest();
		var expectedDtos = new List<CalculatorItemDto> { new() };
		_managerMock.Setup(m => m.GetCalculatorItems(request)).Returns(expectedDtos);

		// Act
		var result = _controller.GetCalculatorItems(request);

		// Assert
		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().BeEquivalentTo(expectedDtos);
	}

	[Fact]
	public void GetCalculator_ShouldReturnOkWithCalculator()
	{
		// Arrange
		var request = new GetCalculatorRequest();
		var expectedDto = new CalculatorDto();
		_managerMock.Setup(m => m.GetCalculator(request)).Returns(expectedDto);

		// Act
		var result = _controller.GetCalculator(request);

		// Assert
		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().BeEquivalentTo(expectedDto);
	}
}
