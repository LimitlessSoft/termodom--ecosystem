using FluentAssertions;
using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Partneri;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Partneri;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class PartnerControllerTests
{
	private readonly Mock<IPartnerManager> _managerMock;
	private readonly PartnerController _controller;

	public PartnerControllerTests()
	{
		_managerMock = new Mock<IPartnerManager>();
		_controller = new PartnerController(_managerMock.Object);
	}

	[Fact]
	public void GetMultiple_ReturnsOk()
	{
		var request = new PartneriGetMultipleRequest();
		var expected = new LSCoreSortedAndPagedResponse<PartnerDto>();
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

		var result = _controller.GetMultiple(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expected);
	}

	[Fact]
	public void Create_ReturnsOk()
	{
		var request = new PartneriCreateRequest();
		_managerMock.Setup(m => m.Create(request)).Returns(123);

		var result = _controller.Create(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(123);
	}

	[Fact]
	public void GetDuplikat_ReturnsOk()
	{
		var request = new PartneriGetDuplikatRequest();
		_managerMock.Setup(m => m.GetDuplikat(request)).Returns(true);

		var result = _controller.GetDuplikat(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(true);
	}

	[Fact]
	public void GetKategorije_ReturnsOk()
	{
		var expected = new List<PPKategorija>();
		_managerMock.Setup(m => m.GetKategorije()).Returns(expected);

		var result = _controller.GetKategorije();

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expected);
	}

	[Fact]
	public void GetPoslednjiId_ReturnsOk()
	{
		_managerMock.Setup(m => m.GetPoslednjiId()).Returns(123);

		var result = _controller.GetPoslednjiId();

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(123);
	}

	[Fact]
	public void GetSingle_ReturnsOk()
	{
		var request = new LSCoreIdRequest { Id = 1 };
		var expected = new PartnerDto();
		_managerMock.Setup(m => m.GetSingle(request)).Returns(expected);

		var result = _controller.GetSingle(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expected);
	}

	[Fact]
	public void GetCount_ReturnsOk()
	{
		_managerMock.Setup(m => m.GetCount()).Returns(100);

		var result = _controller.GetCount();

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(100);
	}
}
