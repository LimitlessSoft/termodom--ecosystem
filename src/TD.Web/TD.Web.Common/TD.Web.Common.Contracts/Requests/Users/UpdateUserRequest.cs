using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Requests.Users
{
    public class UpdateUserRequest : LSCoreSaveRequest
    {
        public string Username { get; set; }
        public string Nickname { get; set; }
        public int? ProfessionId { get; set; }
        public int? PPID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string? Mail { get; set; }
        public int FavoriteStoreId { get; set; }
        public string? Comment { get; set; }
        public UserType Type { get; set; }
    }
}
