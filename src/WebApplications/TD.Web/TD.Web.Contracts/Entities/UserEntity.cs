using TD.Core.Contracts.Entities;

namespace TD.Web.Contracts.Entities
{
    public class UserEntity : Entity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}
