using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings;

public class UserPermissionEntityMap : LSCoreEntityMap<UserPermissionEntity>
{
    public override Action<EntityTypeBuilder<UserPermissionEntity>> Mapper { get; } = entityTypeBuilder =>
    {
        entityTypeBuilder
            .HasOne(x => x.User)
            .WithMany(x => x.Permissions)
            .HasForeignKey(x => x.UserId);

        entityTypeBuilder
            .Property(x => x.UserId)
            .IsRequired();
    };
}