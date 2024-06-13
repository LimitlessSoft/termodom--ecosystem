using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

[Table("Stores")]
public class StoreEntity : LSCoreEntity
{
    public string Name { get; set; }

    [NotMapped]
    public List<UserEntity> Users { get; set; }
}