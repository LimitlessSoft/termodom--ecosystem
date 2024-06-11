using System.ComponentModel.DataAnnotations.Schema;
using TD.Office.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Office.Common.Contracts.Entities
{
    public class UserPermissionEntity : LSCoreEntity
    {
        public Permission Permission { get; set; }
        public long UserId { get; set; }
        
        [NotMapped]
        public UserEntity? User { get; set; }
    }
}