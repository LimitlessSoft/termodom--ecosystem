using FluentAssertions;
using Moq;
using TD.Komercijalno.Api.Controllers;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Procedure;
using Xunit;

namespace TD.Komercijalno.Tests.ControllerTests;

public class ProcedureControllerTests
{
	private readonly Mock<IProcedureManager> _managerMock;
	private readonly ProcedureController _controller;

	public ProcedureControllerTests()
	{
		_managerMock = new Mock<IProcedureManager>();
		_controller = new ProcedureController(_managerMock.Object);
	}

	[Fact]
	public void ProdajnaCenaNaDan_ReturnsDouble()
	{
		var request = new ProceduraGetProdajnaCenaNaDanRequest { Datum = DateTime.Now };
		_managerMock.Setup(m => m.GetProdajnaCenaNaDan(request)).Returns(10.5);

		var result = _controller.ProdajnaCenaNaDan(request);

		result.Should().Be(10.5);
	}

	[Fact]
	public void ProdajnaCenaNaDanOptimized_ReturnsList()
	{
		var request = new ProceduraGetProdajnaCenaNaDanOptimizedRequest { Datum = DateTime.Now };
		var expected = new List<ProdajnaCenaNaDanDto> { new() };
		_managerMock.Setup(m => m.GetProdajnaCenaNaDanOptimized(request)).Returns(expected);

		var result = _controller.ProdajnaCenaNaDanOptimized(request);

		result.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void NabavnaCenaNaDan_ReturnsList()
	{
		var request = new ProceduraGetNabavnaCenaNaDanRequest { Datum = DateTime.Now };
		var expected = new List<NabavnaCenaNaDanDto> { new() };
		_managerMock.Setup(m => m.GetNabavnaCenaNaDan(request)).Returns(expected);

		var result = _controller.NabavnaCenaNaDan(request);

		result.Should().BeEquivalentTo(expected);
	}
}
