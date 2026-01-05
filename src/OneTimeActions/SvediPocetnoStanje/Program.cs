using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Domain.Managers;
using TD.Komercijalno.Repository.Repositories;

var bazaConnString = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
Console.WriteLine("===");
Console.WriteLine("Baza: " + bazaConnString);
Console.WriteLine("===");
Console.WriteLine(
	"Ova akcija ce svesti kolicine u pocetnom/im stanju/ima prema odredjenom kriterijumu:"
);
Console.WriteLine("1. Svim stavkama na minimalno moguce");
Console.WriteLine("2. Samo onoj robi sa karticama u minusu");
Console.WriteLine("0. Izadji");
Console.WriteLine(
	"Napomena: stavke pocetnog stanja nece biti izbrisane, vec samo kolicine azurirane da bi cene bile zdrzane"
);
Console.WriteLine("Napomena: OBAVEZNO SREDI KARTICE PRE PUSTANJA OVE AKCIJE");
Console.Write("Izaberi opciju: ");
var source = new DB(
	$"data source=4monitor; initial catalog = {bazaConnString}; user=SYSDBA; password=m"
);
using var ctx = source.CreateContext();
StavkaManager.DoNotValidate = true;
ProcedureManager.DoNotValidate = true;
var stavkaManager = new StavkaManager(
	NullLogger<StavkaManager>.Instance,
	ctx,
	new DokumentRepository(ctx),
	new MagacinRepository(ctx),
	new RobaRepository(ctx),
	new StavkaRepository(ctx, NullLogger<StavkaRepository>.Instance),
	new ProcedureManager(NullLogger<ProcedureManager>.Instance, ctx)
);
var sifarnik = ctx.Roba.ToDictionary(x => x.Id, x => x);
string opt = string.Empty;
while (string.IsNullOrWhiteSpace(opt))
{
	opt = Console.ReadLine();
	switch (opt)
	{
		case "1":
			Console.WriteLine("Nije implementirano!");
			break;
		case "2":
            SvediSamoOnojRobiSaKarticamaUMinusu();
            break;
		case "0":
			return;
		default:
			Console.WriteLine("Nepoznata opcija!");
			break;
	}
}
Console.WriteLine("====");
Console.WriteLine("====");
Console.WriteLine("====");
Console.WriteLine();
Console.WriteLine("Gotovo!");
Console.WriteLine("Pritisni bilo koje dugme da izadjes!");
Console.WriteLine();
Console.WriteLine("====");
Console.WriteLine("====");
Console.WriteLine("====");
Console.Read();

List<int> IzborMagacina()
{
	Console.Write("Unesi magacine za koje zelis da se ova akcija izvrsi (primer: 112, 115, 118): ");
	var unos = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(unos))
	{
		Console.WriteLine("Neispravan unos magacina!");
		return IzborMagacina();
	}
	var magacini = new List<int>();
	var parts = unos.Split(",");
	foreach (var part in parts)
	{
		int magacin;
		if (!int.TryParse(part.Trim(), out magacin))
		{
			Console.WriteLine($"Neispravan magacin {part}. Ponovite ceo unos!");
			return IzborMagacina();
		}

		var magacinDb = ctx.Magacini.FirstOrDefault(x => x.Id == magacin);
		if (magacinDb == null)
		{
			Console.WriteLine(
				$"Magacin sa ID-em {magacin} nije pronadjen u bazi! Ponovite ceo unos!"
			);
			return IzborMagacina();
		}
		magacini.Add(magacin);
	}
	return magacini;
}
void Log(int magacinId, string message, bool inline = false)
{
	Console.Write($"[{magacinId}] {message}");
	if (!inline)
		Console.WriteLine();
}
void SvediSamoOnojRobiSaKarticamaUMinusu()
{
	Console.WriteLine();
	Console.WriteLine(
		"Ova akcija ce proci kroz svu robu, proveriti karticu i ako ide u minus, za tu robu ce azurirati/insertovati u pocetno stanje kolicinu tako da kartica ne ide u minus."
	);
	var magacini = IzborMagacina();
	foreach (var magacin in magacini)
	{
		Log(magacin, "Trazim pocetno stanje...");
		var pocetnoStanjeMagacina = ctx
			.Dokumenti.Where(x => x.VrDok == 0 && x.MagacinId == magacin)
			.Include(x => x.Stavke)
			.FirstOrDefault();
		if (pocetnoStanjeMagacina == null)
		{
			Log(magacin, $"Magacin {magacin} nema dokument pocetnog stanja! Prekidam akciju.");
			return;
		}
		Log(magacin, "Ucitavam robu u magacinu u memoriju...");
		var robaUMagacinu = ctx.RobaUMagacinu.Where(x => x.MagacinId == magacin).ToList();
		Log(magacin, "Ucitavam sve stavke za ovaj magacin u memoriju...");
		var stavke = ctx.Stavke.Where(x => x.MagacinId == magacin).ToList();
		foreach (var rum in robaUMagacinu)
		{
			var roba = sifarnik[rum.RobaId];
			Log(magacin, $"Analiziram [{roba.KatBr}] {roba.Naziv}...", true);
			var stavkeRobe = stavke.Where(x => x.RobaId == rum.RobaId);
            var minimalnoStanje = stavkeRobe.Any() ? stavkeRobe.Min(x => x.TrenStanje) : 0;
			var stavkaUPocetnomStanju = pocetnoStanjeMagacina.Stavke.FirstOrDefault(x =>
				x.RobaId == rum.RobaId
			);
			var trenutnaKolicinaUPocetnom = stavkaUPocetnomStanju?.Kolicina ?? 0;
			var realnoMinimalnoStanjeKadaBiUPocetnomKolicinaBilaNula =
				minimalnoStanje - trenutnaKolicinaUPocetnom;
			if (realnoMinimalnoStanjeKadaBiUPocetnomKolicinaBilaNula >= 0 || trenutnaKolicinaUPocetnom >= Math.Abs(realnoMinimalnoStanjeKadaBiUPocetnomKolicinaBilaNula))
			{
				Console.WriteLine("OK");
				continue;
			}
            Console.WriteLine($"Nije ok ({minimalnoStanje}). Resavam...");
			var potrebnaKolicina = Math.Abs(realnoMinimalnoStanjeKadaBiUPocetnomKolicinaBilaNula);
			if (stavkaUPocetnomStanju == null)
			{
				stavkaManager.Create(
					new StavkaCreateRequest()
					{
						RobaId = rum.RobaId,
						VrDok = pocetnoStanjeMagacina.VrDok,
						BrDok = pocetnoStanjeMagacina.BrDok,
						Kolicina = potrebnaKolicina,
					}
				);
			}
			else
			{
				if (stavkaUPocetnomStanju.Kolicina == potrebnaKolicina)
					continue;
				stavkaUPocetnomStanju.Kolicina = potrebnaKolicina;
				ctx.SaveChanges();
			}
		}
	}
}

// SELECT ROBAID, MIN(TREN_STANJE) AS MIN_STANJE FROM STAVKA
// WHERE MAGACINID = @MID
// AND VRDOK IN (
//     1,
//     2,
//     3,
//     11,
//     12,
//     16,
//     18,
//     22,
//     26,
//     30,
//     992,
//     13,
//     14,
//     15,
//     17,
//     19,
//     23,
//     28,
//     29,
//     35,
//     993)
// GROUP BY(ROBAID) ORDER BY MIN_STANJE ASC
