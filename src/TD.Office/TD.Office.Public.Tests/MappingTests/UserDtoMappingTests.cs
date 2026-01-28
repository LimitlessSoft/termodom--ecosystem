using FluentAssertions;
using LSCore.Mapper.Domain;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Users;
using Xunit;

namespace TD.Office.Public.Tests.MappingTests;

public class UserDtoMappingTests : TestBase
{
	[Fact]
	public void ToMapped_MapsPpidZaNarudzbenicu()
	{
		// Arrange
		var entity = new UserEntity
		{
			Id = 1,
			Username = "testuser",
			Nickname = "Test User",
			Password = "hashed",
			MaxRabatMPDokumenti = 10,
			MaxRabatVPDokumenti = 20,
			StoreId = 5,
			VPMagacinId = 10,
			TipKorisnikaId = 1,
			PPIDZaNarudzbenicu = 42
		};

		// Act
		var dto = entity.ToMapped<UserEntity, UserDto>();

		// Assert
		dto.PpidZaNarudzbenicu.Should().Be(42);
	}

	[Fact]
	public void ToMapped_WhenPpidZaNarudzbenicuIsNull_MapsNull()
	{
		// Arrange
		var entity = new UserEntity
		{
			Id = 1,
			Username = "testuser",
			Nickname = "Test User",
			Password = "hashed",
			PPIDZaNarudzbenicu = null
		};

		// Act
		var dto = entity.ToMapped<UserEntity, UserDto>();

		// Assert
		dto.PpidZaNarudzbenicu.Should().BeNull();
	}
}
