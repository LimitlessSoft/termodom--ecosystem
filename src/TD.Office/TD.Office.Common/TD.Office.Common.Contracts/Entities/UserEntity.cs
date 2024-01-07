using LSCore.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities
{
    public class UserEntity : LSCoreEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public UserType Type { get; set; }
    }
}
