using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Web.Common.Repository;

namespace TD.Web.Public.Tests;

public class TestWebDbContext(
	DbContextOptions<WebDbContext> options,
	IConfigurationRoot configurationRoot
) : WebDbContext(options, configurationRoot)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		// Overriding to avoid Npgsql setup in tests
	}
}
