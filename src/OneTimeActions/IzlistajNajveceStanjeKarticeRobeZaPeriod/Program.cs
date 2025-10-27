using FirebirdSql.Data.FirebirdClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IzlistajNajveceStanjeKarticeRobeZaPeriod
{
    internal class Program
    {
        static string production2025tcmd = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
        static string production2024tcmd = "C:\\Poslovanje\\Baze\\2024\\FRANSIZA2024TCMD.FDB";
        static string production2025vhemza = "C:\\Poslovanje\\Baze\\Vhemza\\2025\\VHEMZA2025.FDB";

        static string develop2025tcmd =
            "C:\\Poslovanje\\Baze\\develop\\2025\\FRANSIZA2025TCMD-develop.FDB";
        static string develop2025td = "C:\\Poslovanje\\Baze\\develop\\2025\\TERMODOM2025-develop.FDB";
        static string develop2025vhemza = "C:\\Poslovanje\\Baze\\develop\\2025\\VHEMZA2025-develop.FDB";

        static string sourceString = production2024tcmd;
        static readonly DB source = new DB(
            $"data source=4monitor; initial catalog = {sourceString}; user=SYSDBA; password=m"
        );

        static int magacinId = 112;
        static DateTime odDatumaInclusive = new DateTime(2023, 1, 1);
        static DateTime doDatumaInclusive = new DateTime(2026, 2, 2);
        static void Main(string[] args)
        {
            try
            {
                using var ctx = source.CreateContext();
                //ctx.Database.ExecuteSqlRaw(@"
                //    EXECUTE BLOCK AS
                //    BEGIN
                //        RDB$SET_CONTEXT('USER_SESSION', 'USER', 'NO_TRIGG');
                //    END");
                var stavka = ctx.Stavke.First(x => x.Id == 19454);
                stavka.Kolicina = -1;
                ctx.Stavke.Update(stavka);
                ctx.SaveChanges();
                Console.WriteLine($"Updating stavka {stavka.Id}");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return;
            Console.WriteLine("Startujem...");
            using FbConnection connectionPure = new FbConnection($"data source=4monitor; initial catalog = {sourceString}; user=SYSDBA; password=m");
            connectionPure.Open();
            using var context = source.CreateContext();
            Console.WriteLine("Uzimam podatke iz baze...");
            var roba = context.Roba.ToDictionary(x => x.Id, x => x);
            var robaUMagacinu = context.RobaUMagacinu.Where(x => x.MagacinId ==  magacinId);
            var report = new List<Dto>();

            foreach (var r in robaUMagacinu)
            {
                Console.WriteLine($"Racunam karticu za: {roba[r.RobaId].Naziv}");
                try
                {
                    List<
                        Tuple<int, double>
                    > vrdokIKolicinaIzTabeleStavkaPoredjaneRedosledomZaDatiRobaID =
                        new List<Tuple<int, double>>();
                    using (
                        FbCommand cmd = new FbCommand(
                            @"SELECT D.VRDOK, S.KOLICINA, D.DATUM FROM STAVKA S
LEFT OUTER JOIN DOKUMENT D ON S.VRDOK = D.VRDOK AND S.BRDOK = D.BRDOK
WHERE S.ROBAID = @RID AND D.VRDOK IN ("
                                + string.Join(", ", vrDokKojiPovecavajuStanje)
                                + string.Join(", ", vrDokKojiSmanjujuStanje)
                                + @") AND D.MAGACINID = @MID
ORDER BY D.DATUM ASC, D.LINKED"
                    , connectionPure)
                    )
                    {
                        cmd.Parameters.AddWithValue("@MID", magacinId);
                        cmd.Parameters.AddWithValue("@RID", r.RobaId);

                        using (FbDataReader dr = cmd.ExecuteReader())
                            while (dr.Read())
                            {
                                var dat = Convert.ToDateTime(dr[2]);
                                if (dat.Date < odDatumaInclusive || dat.Date > doDatumaInclusive)
                                    continue;
                                vrdokIKolicinaIzTabeleStavkaPoredjaneRedosledomZaDatiRobaID.Add(
                                    new Tuple<int, double>(
                                        Convert.ToInt32(dr[0]),
                                        Convert.ToDouble(dr[1])
                                    )
                                );
                            }
                    }

                    double trenutnoStanje = 0;
                    double minimalnoStanje = Int32.MaxValue;

                    foreach (
                        Tuple<
                            int,
                            double
                        > t in vrdokIKolicinaIzTabeleStavkaPoredjaneRedosledomZaDatiRobaID
                    )
                    {
                        trenutnoStanje += vrDokKojiPovecavajuStanje.Contains(t.Item1)
                            ? t.Item2
                            : t.Item2 * -1;
                        if (trenutnoStanje < minimalnoStanje)
                            minimalnoStanje = trenutnoStanje;
                    }
                    if (minimalnoStanje == Int32.MaxValue)
                        continue;
                    if (minimalnoStanje == 0)
                        continue;

                    var rob = roba[r.RobaId];
                    report.Add(new Dto()
                    {
                        KatBr = rob.KatBr,
                        Naziv = rob.Naziv,
                        Kolicina = minimalnoStanje
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            var sb = new StringBuilder();
            foreach (var dto in report.Where(x => x.Kolicina != 0).OrderByDescending(x => x.Kolicina))
                sb.AppendLine($"[{dto.KatBr}] {dto.Naziv} ima minimalnu kolicinu: {dto.Kolicina}");
            Console.WriteLine(sb.ToString());
            Console.WriteLine("Izvestaj je gotov!");
            var fileName = "izvestaj-najmanjeg-stanja-za-period.txt";
            System.IO.File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName), sb.ToString());
            Console.WriteLine("Izvestaj je sacuvan na desktopu sa nazivom: " + fileName);
        }

        static List<int> vrDokKojiPovecavajuStanje = new List<int>()
        {
            0,
            1,
            2,
            3,
            11,
            12,
            16,
            18,
            22,
            26,
            30,
            992
        };
        static List<int> vrDokKojiSmanjujuStanje = new List<int>()
        {
            13,
            14,
            15,
            17,
            19,
            23,
            28,
            29,
            35,
            993
        };
    }
}
internal class Dto
{
    public string KatBr { get; set; }
    public string Naziv { get; set; }
    public double Kolicina { get; set; }
}
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