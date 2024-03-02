using LSCore.Contracts.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities
{
    public class UserEntity : LSCoreEntity
    {
        public int CityId { get; set; }
        public string? Mail { get; set; }
        public string Mobile { get; set; }
        public UserType Type { get; set; }
        public string Address { get; set; }
        public int? ReferentId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public int FavoriteStoreId { get; set; }
        public DateTime DateOfBirth {  get; set; }
        public DateTime? LastTimeSeen { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public int? ProfessionId { get; set; }
        public string? PIB { get; set; }
        public int? PPID { get; set; }
        public string? Comment { get; set; }

        [NotMapped]
        public List<OrderEntity> Orders { get; set; }
        [NotMapped]
        public List<ProductPriceGroupLevelEntity> ProductPriceGroupLevels { get; set; }
        [NotMapped]
        public CityEntity City { get; set; }
        [NotMapped]
        public StoreEntity FavoriteStore { get; set; }
        [NotMapped]
        public ProfessionEntity? Profession { get; set; }
        [NotMapped]
        public UserEntity? Referent { get; set; }
    }
}
