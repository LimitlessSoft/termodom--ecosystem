using TD.Core.Contracts;

namespace TD.Web.Contracts.Entities
{
    public class UserEntity : IEntity
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}
