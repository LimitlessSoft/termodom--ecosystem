using Omu.ValueInjecter;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.Products
{
    public static class ProductsGetMultipleDtoMappings
    {
        public static List<ProductsGetMultipleDto> ToDtoList(this List<ProductEntity> sender)
        {
            var list = new List<ProductsGetMultipleDto>();
            foreach(var entity in sender)
            {
                var dtoItem = new ProductsGetMultipleDto();
                dtoItem.InjectFrom(entity);
                dtoItem.Groups = entity.Groups.ToDtoList();
                list.Add(dtoItem);
            }
            return list;
        }

        public static List<ProductsGetMultipleGroupItemDto> ToDtoList(this List<ProductGroupEntity> sender)
        {
            var list = new List<ProductsGetMultipleGroupItemDto>();
            foreach(var entity in sender)
            {
                var dtoItem = new ProductsGetMultipleGroupItemDto();
                dtoItem.InjectFrom(entity);
                list.Add(dtoItem);
            }
            return list;
        }
    }
}
