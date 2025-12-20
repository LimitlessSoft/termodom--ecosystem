using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Office.Public.Api.Controllers;
using TD.Office.Public.Contracts.Dtos.Popisi;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using Xunit;

namespace TD.Office.Public.Tests.ControllerTests;

public class PopisiControllerTests
{
	private readonly Mock<IPopisManager> _popisManagerMock = new();
	private readonly PopisiController _controller;

	public PopisiControllerTests()
	{
		_controller = new PopisiController(_popisManagerMock.Object);
	}

	[Fact]
	public void GetById_ReturnsOkWithDto()
	{
		// Arrange
		var id = 1L;
		var dto = new PopisDetailedDto { Id = id };
		_popisManagerMock.Setup(m => m.GetById(id)).Returns(dto);

		// Act
		var result = _controller.GetById(id);

		// Assert
		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(dto);
	}
}
