using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Requests.Users
{
    public class UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int CityId { get; set; }
        public int FavoriteStoreId { get; set; }
        public string? Mail { get; set; }
    }
}
