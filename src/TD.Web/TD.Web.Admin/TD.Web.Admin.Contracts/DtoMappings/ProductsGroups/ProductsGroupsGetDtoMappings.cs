using Omu.ValueInjecter;
using TD.Core.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductsGroups
{
    public class ProductsGroupsGetDtoMappings : IDtoMapper<ProductsGroupsGetDto, ProductGroupEntity>
    {
        public ProductsGroupsGetDto ToDto(ProductGroupEntity sender)
        {
            var dto = new ProductsGroupsGetDto();
            dto.InjectFrom(sender);
            return dto;
        }

        public List<ProductsGroupsGetDto> ToDtoList(List<ProductGroupEntity> sender)
        {
            var list = new List<ProductsGroupsGetDto>();
            foreach (var entity in sender)
                list.Add(ToDto(entity));
            return list;
        }
    }
}
