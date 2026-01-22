using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Contracts.IRepositories;

public interface IOfficeDbContextFactory
{
	IOfficeDbContext Create();
}

public interface IOfficeDbContext : IDisposable
{
	DbSet<MagacinCentarEntity> MagacinCentri { get; }
}
