using Omu.ValueInjecter;
using System.Collections.Generic;
using TD.Web.Contracts.Dtos.ProductsGroups;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.ProductsGroups
{
    public static class ProductsGroupsGetDtoMappings
    {
        public static ProductsGroupsGetDto ToDto(this ProductGroupEntity sender)
        {
            var dto = new ProductsGroupsGetDto();

            dto.ParentGroupId = sender.ParentGroup?.Id;

            dto.InjectFrom(sender);
            return dto;
        }

        public static List<ProductsGroupsGetDto> ToDtoList(this List<ProductGroupEntity> sender)
        {
            var list = new List<ProductsGroupsGetDto>();
            foreach (var entity in sender)
                list.Add(entity.ToDto());
            return list;
        }
    }
}
