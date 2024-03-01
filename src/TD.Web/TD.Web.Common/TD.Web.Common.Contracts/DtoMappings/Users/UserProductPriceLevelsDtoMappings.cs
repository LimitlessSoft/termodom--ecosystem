using LSCore.Contracts.Http;
using LSCore.Contracts.Interfaces;
using System.Runtime.CompilerServices;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Users
{
    public static class UserProductPriceLevelsDtoMappings
    {
        public static List<UserProductPriceLevelsDto> ToUserPriceLevelsDto(this List<ProductPriceGroupLevelEntity> sender)
        {
            var response = new List<UserProductPriceLevelsDto>();

            foreach (var item in sender)
                response.Add(new UserProductPriceLevelsDto()
                {
                    GroupId = item.ProductPriceGroupId,
                    Level = item.Level
                });

            return response;
        }
    }
}
