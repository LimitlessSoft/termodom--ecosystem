using FluentAssertions;
using Moq;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Public.Api.Controllers;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using Xunit;

namespace TD.Web.Public.Tests.ControllerTests;

public class EnumsControllerTests
{
	private readonly Mock<IEnumManager> _managerMock;
	private readonly EnumsController _controller;

	public EnumsControllerTests()
	{
		_managerMock = new Mock<IEnumManager>();
		_controller = new EnumsController(_managerMock.Object);
	}

	[Fact]
	public void GetProductStockTypes_ShouldReturnTypes()
	{
		// Arrange
		var expectedDtos = new List<IdNamePairDto> { new() { Id = 1, Name = "Test" } };
		_managerMock.Setup(m => m.GetProductStockTypes()).Returns(expectedDtos);

		// Act
		var result = _controller.GetProductStockTypes();

		// Assert
		result.Should().BeEquivalentTo(expectedDtos);
	}
}
