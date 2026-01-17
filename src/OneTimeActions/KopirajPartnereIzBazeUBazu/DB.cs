using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Repository;

namespace KopirajPartnereIzBazeUBazu
{
	internal class DB
	{
		readonly DbContextOptionsBuilder<KomercijalnoDbContext> _optionsBuilder =
			new DbContextOptionsBuilder<KomercijalnoDbContext>();

		public DB(string connectionString)
		{
			_optionsBuilder.UseFirebird(connectionString);
		}

		public KomercijalnoDbContext CreateContext() =>
			new KomercijalnoDbContext(_optionsBuilder.Options);
	}
}
