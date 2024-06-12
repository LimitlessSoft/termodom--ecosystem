using System.ComponentModel.DataAnnotations.Schema;
using TD.Office.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Office.Common.Contracts.Entities
{
    public class UserEntity : LSCoreEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public UserType Type { get; set; }
        public int? StoreId { get; set; }
        
        [NotMapped]
        public List<UserPermissionEntity>? Permissions { get; set; }
    }
}
