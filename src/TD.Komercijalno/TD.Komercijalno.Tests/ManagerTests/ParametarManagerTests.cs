using Moq;
using TD.Komercijalno.Contracts.Interfaces.IRepositories;
using TD.Komercijalno.Contracts.Requests.Parametri;
using TD.Komercijalno.Domain.Managers;
using Xunit;

namespace TD.Komercijalno.Tests.ManagerTests;

public class ParametarManagerTests
{
	private readonly Mock<IParametarRepository> _repositoryMock;
	private readonly ParametarManager _manager;

	public ParametarManagerTests()
	{
		_repositoryMock = new Mock<IParametarRepository>();
		_manager = new ParametarManager(_repositoryMock.Object);
	}

	[Fact]
	public void Update_CallsRepository()
	{
		// Arrange
		var request = new UpdateParametarRequest { Naziv = "P1", Vrednost = "V1" };

		// Act
		_manager.Update(request);

		// Assert
		_repositoryMock.Verify(r => r.SetVrednost("P1", "V1"), Times.Once);
	}
}
