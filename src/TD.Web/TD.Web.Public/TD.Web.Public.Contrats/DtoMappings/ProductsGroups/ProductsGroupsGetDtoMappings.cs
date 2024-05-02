using Omu.ValueInjecter;
using LSCore.Contracts.Dtos;
using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;

namespace TD.Web.Public.Contracts.DtoMappings.ProductsGroups
{
    public class ProductsGroupsGetDtoMappings : ILSCoreDtoMapper<ProductsGroupsGetDto, ProductGroupEntity>
    {
        public ProductsGroupsGetDto ToDto(ProductGroupEntity sender)
        {
            var dto = new ProductsGroupsGetDto();
            dto.InjectFrom(sender);
            dto.ParentName = sender.ParentGroup?.Name;
            dto.WelcomeMessage = sender.WelcomeMessage;
            dto.Type = sender.Type;
            return dto;
        }
    }
}
