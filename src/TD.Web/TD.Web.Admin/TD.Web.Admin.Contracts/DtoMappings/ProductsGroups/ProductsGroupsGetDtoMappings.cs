using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.ProductsGroups
{
    public class ProductsGroupsGetDtoMappings : ILSCoreDtoMapper<ProductsGroupsGetDto, ProductGroupEntity>
    {
        public ProductsGroupsGetDto ToDto(ProductGroupEntity sender)
        {
            var dto = new ProductsGroupsGetDto();
            dto.InjectFrom(sender);
            dto.TypeId = (int)sender.Type;
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
