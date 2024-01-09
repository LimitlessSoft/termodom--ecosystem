using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository.EntityMappings;

namespace TD.Office.Common.Repository
{
    public class OfficeDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public OfficeDbContext(DbContextOptions otpions) : base(otpions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().AddMap(new UserEntityMap());
        }
    }
}
