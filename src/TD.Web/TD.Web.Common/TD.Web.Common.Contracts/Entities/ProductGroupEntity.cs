using System.ComponentModel.DataAnnotations.Schema;
using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Web.Common.Contracts.Entities;

public class ProductGroupEntity : LSCoreEntity
{
    public string Name { get; set; }
    public long? ParentGroupId { get; set; }
    public string? WelcomeMessage { get; set; }
    public ProductGroupType Type { get; set; }
    public string? SalesMobile { get; set; }

    [NotMapped]
    public List<ProductEntity> Products { get; set; }

    [NotMapped]
    public ProductGroupEntity? ParentGroup { get; set; }
}