using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Products
{
    public static class ProductsGetDtoMappings
    {
        public static ProductsGetDto ToDto(this ProductEntity sender)
        {
            var dto = new ProductsGetDto();
            dto.InjectFrom(sender);
            if(sender.Groups != null)
                dto.Groups = sender.Groups.Select(z => z.Id).ToList();

            dto.UnitId = sender.Unit.Id;
            dto.Classification = (int)sender.Classification;
            return dto;
        }

        public static List<ProductsGetDto> ToDtoList(this List<ProductEntity> sender)
        {
            var list = new List<ProductsGetDto>();
            foreach(var entity in sender)
                list.Add(entity.ToDto());
            return list;
        }
    }
}
