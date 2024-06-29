using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class UnitEntity : LSCoreEntity
{
    public string Name { get; set; }

    [NotMapped]
    public List<ProductEntity> Products { get; set; }
    [NotMapped]
    public List<ProductEntity> AlternateProducts { get; set; }
}