using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Office.MassSMS.Contracts.Interfaces;

namespace TD.Office.MassSMS.Repository;

public class MassSMSDbContextFactory(IConfigurationRoot configurationRoot)
	: IMassSMSDbContextFactory
{
	public IMassSMSContext Create()
	{
		return (IMassSMSContext)
			Activator.CreateInstance(
				typeof(MassSMSContext),
				new DbContextOptionsBuilder<MassSMSContext>().Options,
				configurationRoot
			)!;
	}
}
