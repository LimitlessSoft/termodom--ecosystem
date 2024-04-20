using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Dtos.Users
{
    public class UsersGetDto
    {
        public UserType UserTypeId { get; set; }
        public string UserType { get; set; }
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public int FavoriteStoreId { get; set; }
        public int? ProfessionId { get; set; }
        public int CityId { get; set; }
    }
}
