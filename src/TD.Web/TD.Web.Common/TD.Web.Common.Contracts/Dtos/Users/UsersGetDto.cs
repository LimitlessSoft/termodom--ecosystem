namespace TD.Web.Common.Contracts.Dtos.Users
{
    public class UsersGetDto
    {
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Mobile { get; set; }
    }
}
