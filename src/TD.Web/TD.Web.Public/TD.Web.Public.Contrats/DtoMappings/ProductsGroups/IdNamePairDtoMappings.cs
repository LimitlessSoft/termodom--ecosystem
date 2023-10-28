using Omu.ValueInjecter;
using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Public.Contrats.DtoMappings.ProductsGroups
{
    public class IdNamePairDtoMappings : IDtoMapper<IdNamePairDto, ProductGroupEntity>
    {
        public IdNamePairDto ToDto(ProductGroupEntity sender)
        {
            var dto = new IdNamePairDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
