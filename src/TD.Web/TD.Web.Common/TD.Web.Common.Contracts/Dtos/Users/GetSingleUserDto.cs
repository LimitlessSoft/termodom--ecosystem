using LSCore.Contracts.Dtos;

namespace TD.Web.Common.Contracts.Dtos.Users
{
    public class GetSingleUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public LSCoreIdNamePairDto? Profession { get; set; }
        public string? PIB { get; set; }
        public int? PPID { get; set; }
        public DateTime DateOfBirth { get; set; }
        public LSCoreIdNamePairDto City { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string? Mail { get; set; }
        public LSCoreIdNamePairDto FavoriteStore { get; set; }
        public string? Comment { get; set; }
    }
}
