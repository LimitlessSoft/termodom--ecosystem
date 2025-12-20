using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Repository;

namespace TD.Office.Public.Tests;

public class TestOfficeDbContext(
	DbContextOptions<OfficeDbContext> options,
	IConfigurationRoot configurationRoot
) : OfficeDbContext(options, configurationRoot)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// Overriding to avoid Npgsql setup in tests
	}
}
