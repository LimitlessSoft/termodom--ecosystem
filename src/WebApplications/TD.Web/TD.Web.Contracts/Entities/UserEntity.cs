using TD.Core.Contracts.Entities;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Entities
{
    public class UserEntity : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public int? Referent {  get; set; }
        public DateTime? LastTimeSeen { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public DateTime DateOfBirth {  get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int FavoriteStoreId { get; set; }
        public string? Mail { get; set; }
        public UserClassification UserType { get; set; }
    }
}
