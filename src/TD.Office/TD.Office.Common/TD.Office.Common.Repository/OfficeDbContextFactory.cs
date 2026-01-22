using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository;

public class OfficeDbContextFactory(IConfigurationRoot configurationRoot)
	: IOfficeDbContextFactory
{
	public IOfficeDbContext Create()
	{
		return new OfficeDbContext(
			new DbContextOptionsBuilder<OfficeDbContext>().Options,
			configurationRoot
		);
	}
}
