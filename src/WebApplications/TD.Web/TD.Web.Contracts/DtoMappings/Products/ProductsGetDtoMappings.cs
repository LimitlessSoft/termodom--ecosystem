using Omu.ValueInjecter;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.Products
{
    public static class ProductsGetDtoMappings
    {
        public static ProductsGetDto ToDto(this ProductEntity sender)
        {
            var dto = new ProductsGetDto();
            dto.InjectFrom(sender);
            if(sender.Groups != null)
                dto.Groups = sender.Groups.ToDtoList();
            return dto;
        }

        public static List<ProductsGetDto> ToDtoList(this List<ProductEntity> sender)
        {
            var list = new List<ProductsGetDto>();
            foreach(var entity in sender)
                list.Add(entity.ToDto());
            return list;
        }

        public static List<ProductsGetGroupItemDto> ToDtoList(this List<ProductGroupEntity> sender)
        {
            var list = new List<ProductsGetGroupItemDto>();
            foreach(var entity in sender)
            {
                var dtoItem = new ProductsGetGroupItemDto();
                dtoItem.InjectFrom(entity);
                list.Add(dtoItem);
            }
            return list;
        }
    }
}
