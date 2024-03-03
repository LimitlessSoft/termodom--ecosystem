using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Users
{
    public static class UserProductPriceLevelsDtoMappings
    {
        public static List<UserProductPriceLevelsDto> ToUserPriceLevelsDto(this List<ProductPriceGroupLevelEntity> sender, List<ProductPriceGroupEntity> groups) =>
            groups.Select(group => new UserProductPriceLevelsDto()
                {
                    GroupId = group.Id,
                    Level = sender.FirstOrDefault(x => x.ProductPriceGroupId == group.Id)?.Level ?? 0
                })
                .ToList();
    }
}
