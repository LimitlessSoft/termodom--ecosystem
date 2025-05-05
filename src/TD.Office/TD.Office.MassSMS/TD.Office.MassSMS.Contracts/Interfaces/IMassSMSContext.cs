using LSCore.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using TD.Office.MassSMS.Contracts.Entities;

namespace TD.Office.MassSMS.Contracts.Interfaces;

public interface IMassSMSContext : ILSCoreDbContext, IDisposable
{
	DbSet<SMSEntity> SMSs { get; }
	DbSet<SettingEntity> Settings { get; }
	DbSet<NumberEntity> Numbers { get; }
}
