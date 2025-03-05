using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Repository;

namespace KopirajRobaIzaBazeUBazu
{
    internal class Program
    {
        static string production2025tcmd = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
        static string production2025vhemza = "C:\\Poslovanje\\Baze\\Vhemza\\2025\\VHEMZA2025.FDB";

        static string develop2025tcmd = "C:\\Poslovanje\\Baze\\develop\\2025\\FRANSIZA2025TCMD-develop.FDB";
        static string develop2025td = "C:\\Poslovanje\\Baze\\develop\\2025\\TERMODOM2025-develop.FDB";
        static string develop2025vhemza = "C:\\Poslovanje\\Baze\\develop\\2025\\VHEMZA2025-develop.FDB";

        static string sourceString = production2025tcmd;
        static string destinationString = production2025vhemza;

        static readonly DB source = new DB($"data source=4monitor; initial catalog = {sourceString}; user=SYSDBA; password=m");
        static readonly DB destination = new DB($"data source=4monitor; initial catalog = {destinationString}; user=SYSDBA; password=m");

        static void Main(string[] args)
        {
            Console.WriteLine("Starting application");
            var products = source.CreateContext().Roba.ToList();
            CopyTheOnesThatDoesntExist(source.CreateContext(), destination.CreateContext());
        }

        static void CopyTheOnesThatDoesntExist(KomercijalnoDbContext sourceContext, KomercijalnoDbContext destinationContext)
        {
            // Kopiraj robu
            var sourceRoba = sourceContext.Roba.ToDictionary(x => x.Id, x => x);
            var destinationRoba = destinationContext.Roba.ToDictionary(x => x.Id, x => x);
            var sourceRobaIds = sourceRoba.Select(x => x.Key).ToHashSet();
            var destinationRobaIds = destinationRoba.Select(x => x.Key).ToHashSet();

            var robaThatExistsInSourceOnly = new HashSet<int>();
            foreach (var id in sourceRobaIds)
                if (!destinationRobaIds.Contains(id))
                    robaThatExistsInSourceOnly.Add(id);

            Console.WriteLine($"Pronadjeno {robaThatExistsInSourceOnly.Count} entiteta koji ne postoje u destinacionoj bazi.");

            foreach (var id in robaThatExistsInSourceOnly)
            {
                var sourceR = sourceRoba[id];
                var r = new Roba();
                r.InjectFrom(sourceR);
                destinationContext.Roba.Add(r);
            }

            destinationContext.SaveChanges();
        }
    }

    internal class DB
    {
        readonly DbContextOptionsBuilder<KomercijalnoDbContext> _optionsBuilder = new DbContextOptionsBuilder<KomercijalnoDbContext>();
        public DB(string connectionString)
        {
            _optionsBuilder.UseFirebird(connectionString);
        }

        public KomercijalnoDbContext CreateContext() => new KomercijalnoDbContext(_optionsBuilder.Options);
    }
}
