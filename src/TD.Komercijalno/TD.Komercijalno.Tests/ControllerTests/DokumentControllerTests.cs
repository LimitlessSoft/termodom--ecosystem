using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Dokument;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class DokumentControllerTests
{
	private readonly Mock<IDokumentManager> _managerMock;
	private readonly DokumentController _controller;

	public DokumentControllerTests()
	{
		_managerMock = new Mock<IDokumentManager>();
		_controller = new DokumentController(_managerMock.Object);
	}

	[Fact]
	public void Get_ReturnsOk()
	{
		var request = new DokumentGetRequest { VrDok = 1, BrDok = 1 };
		var expected = new DokumentDto();
		_managerMock.Setup(m => m.Get(request)).Returns(expected);

		var result = _controller.Get(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expected);
	}

	[Fact]
	public void GetMultiple_ReturnsOk()
	{
		var request = new DokumentGetMultipleRequest();
		var expected = new List<DokumentDto>();
		_managerMock.Setup(m => m.GetMultiple(request)).Returns(expected);

		var result = _controller.GetMultiple(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expected);
	}

	[Fact]
	public void Create_ReturnsOk()
	{
		var request = new DokumentCreateRequest();
		var expected = new DokumentDto();
		_managerMock.Setup(m => m.Create(request)).Returns(expected);

		var result = _controller.Create(request);

		var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
		okResult.Value.Should().Be(expected);
	}

	[Fact]
	public void SetNacinPlacanja_ReturnsOk()
	{
		var request = new DokumentSetNacinPlacanjaRequest
		{
			VrDok = 1,
			BrDok = 1,
			NUID = 1,
		};

		var result = _controller.SetNacinPlacanja(request);

		result.Should().BeOfType<OkResult>();
		_managerMock.Verify(m => m.SetNacinPlacanja(request), Times.Once);
	}

	[Fact]
	public void UpdateDokOut_ReturnsOk()
	{
		var request = new DokumentSetDokOutRequest { VrDok = 1, BrDok = 1 };

		var result = _controller.UpdateDokOut(request);

		result.Should().BeOfType<OkResult>();
		_managerMock.Verify(m => m.SetDokOut(request), Times.Once);
	}

	[Fact]
	public void SetDokumentFlag_ReturnsOk()
	{
		var request = new DokumentSetFlagRequest
		{
			VrDok = 1,
			BrDok = 1,
			Flag = 1,
		};

		var result = _controller.SetDokumentFlag(request);

		result.Should().BeOfType<OkResult>();
		_managerMock.Verify(m => m.SetFlag(request), Times.Once);
	}
}
