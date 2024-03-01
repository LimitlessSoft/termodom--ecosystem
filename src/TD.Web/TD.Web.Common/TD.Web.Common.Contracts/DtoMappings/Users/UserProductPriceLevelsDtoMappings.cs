using LSCore.Contracts.Http;
using LSCore.Contracts.Interfaces;
using System.Runtime.CompilerServices;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Users
{
    public static class UserProductPriceLevelsDtoMappings
    {
        public static List<UserProductPriceLevelsDto> ToUserPriceLevelsDto(this List<ProductPriceGroupLevelEntity> sender, List<ProductPriceGroupEntity> groups)
        {
            var response = new List<UserProductPriceLevelsDto>();

            foreach (var group in groups)
                response.Add(new UserProductPriceLevelsDto()
                {
                    GroupId = group.Id,
                    Level = 0
                });

            foreach (var item in sender)
                response.Where(x => x.GroupId == item.ProductPriceGroupId).FirstOrDefault()!.Level = item.Level;    

            return response;
        }
    }
}
