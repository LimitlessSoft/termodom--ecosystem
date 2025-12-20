using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.IstorijaUplata;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.IstorijaUplata;
using Xunit;

namespace TD.Komercijalno.Tests;

public class IstorijaUplataControllerTests
{
	private readonly Mock<IIstorijaUplataManager> _managerMock;
	private readonly IstorijaUplataController _controller;

	public IstorijaUplataControllerTests()
	{
		_managerMock = new Mock<IIstorijaUplataManager>();
		_controller = new IstorijaUplataController(_managerMock.Object);
	}

	[Fact]
	public void GetMultiple_ReturnsOk()
	{
		var request = new IstorijaUplataGetMultipleRequest();
		var expected = new List<IstorijaUplataDto>();
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

		var result = _controller.GetMultiple(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expected);
	}
}
