using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings;

public class ModuleHelpEntityMap : LSCoreEntityMap<ModuleHelpEntity>
{
    public override Action<EntityTypeBuilder<ModuleHelpEntity>> Mapper { get; } = (builder) => { };
}
