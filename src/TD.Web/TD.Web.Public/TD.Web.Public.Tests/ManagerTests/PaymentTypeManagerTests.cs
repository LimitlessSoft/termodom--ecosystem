using FluentAssertions;
using Moq;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class PaymentTypeManagerTests : TestBase
{
	private readonly Mock<IPaymentTypeRepository> _repositoryMock;
	private readonly PaymentTypeManager _manager;

	public PaymentTypeManagerTests()
	{
		_repositoryMock = new Mock<IPaymentTypeRepository>();
		_manager = new PaymentTypeManager(_repositoryMock.Object);
	}

	[Fact]
	public void GetMultiple_ShouldReturnMappedPaymentTypes()
	{
		// Arrange
		var entities = new List<PaymentTypeEntity>
		{
			new() { Id = 1, Name = "Test" },
		};
		_repositoryMock.Setup(r => r.GetMultiple()).Returns(entities.AsQueryable());

		// Act
		var result = _manager.GetMultiple();

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(1);
		result[0].Id.Should().Be(1);
		result[0].Name.Should().Be("Test");
	}
}
