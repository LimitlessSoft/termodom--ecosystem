using LSCore.Contracts.Dtos;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Public.Contrats.DtoMappings.ProductsGroups
{
    public class IdNamePairDtoMappings : ILSCoreDtoMapper<LSCoreIdNamePairDto, ProductGroupEntity>
    {
        public LSCoreIdNamePairDto ToDto(ProductGroupEntity sender)
        {
            var dto = new LSCoreIdNamePairDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
