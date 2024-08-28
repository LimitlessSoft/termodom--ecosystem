using LSCore.Contracts.Dtos;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Users;

public class GetSingleUserDtoMappings : ILSCoreDtoMapper<UserEntity, GetSingleUserDto>
{
    public GetSingleUserDto ToDto(UserEntity sender)
    {
        var dto = new GetSingleUserDto();

        dto.InjectFrom(sender);

        dto.Profession =
            sender.Profession == null
                ? null
                : new LSCoreIdNamePairDto()
                {
                    Id = sender.Profession.Id,
                    Name = sender.Profession.Name,
                };
        dto.City = new LSCoreIdNamePairDto() { Id = sender.City.Id, Name = sender.City.Name, };
        dto.FavoriteStore = new LSCoreIdNamePairDto()
        {
            Id = sender.FavoriteStore.Id,
            Name = sender.FavoriteStore.Name,
        };

        dto.Status = sender.IsActive
            ? sender.ProcessingDate == null
                ? "Na obradi"
                : "Aktivan"
            : "Deaktiviran";
        dto.HasOwner = sender.Referent != null;
        dto.Referent = sender.Referent?.Nickname ?? "bez referenta";
        dto.DefaultPaymentTypeId = sender.DefaultPaymentTypeId;

        return dto;
    }
}
