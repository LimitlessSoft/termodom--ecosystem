using FluentAssertions;
using TD.Web.Public.Domain.Managers;
using Xunit;

namespace TD.Web.Public.Tests.ManagerTests;

public class EnumManagerTests
{
	private readonly EnumManager _manager;

	public EnumManagerTests()
	{
		_manager = new EnumManager();
	}

	[Fact]
	public void GetProductStockTypes_ShouldReturnValues()
	{
		// Act
		var result = _manager.GetProductStockTypes();

		// Assert
		result.Should().NotBeEmpty();
		result.Should().Contain(x => x.Name != null);
	}
}
