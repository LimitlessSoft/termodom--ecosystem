using TD.TDOffice.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using LSCore.Repository;

namespace TD.TDOffice.Repository;

public class TDOfficeDbContext : LSCoreDbContext<TDOfficeDbContext>
{
    public DbSet<DokumentTagIzvod> DokumentTagIzvodi { get; set; }
    public DbSet<MCPartnerCenovnikKatBrRobaIdEntity> MCPartnerCenovnikKatBrRobaIds { get; set; }
    public DbSet<MCPartnerCenovnikItemEntity> MPPartnerCenovnikItems { get; set; }

    public TDOfficeDbContext(DbContextOptions<TDOfficeDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}