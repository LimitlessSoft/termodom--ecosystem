using Microsoft.EntityFrameworkCore;
using NeslaganjaUInternomTransportu;
using System.Text;
using TD.Komercijalno.Contracts.Entities;

var bazaConnString = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
Console.WriteLine("===");
Console.WriteLine("Baza: " + bazaConnString);
Console.WriteLine("===");
Console.WriteLine(
	"Izlistavam neslaganja stavki u dokumentima internog transporta po sledecem kriterijumu:"
);
Console.WriteLine("1. Osnovna neslaganja (broj stavki, roba, kolicine)");
Console.WriteLine("2. Gde u stavkama izlaznog dokumenta postoji prodajna cena 0");
Console.WriteLine("3. Gde u stavkama izlaznog dokumenta postoji nabavna cena 0");
Console.WriteLine(
	"4. Gde je razlika izmedju nabavne cene manja ili veca od X% u stavkama izlaznog dokumenta"
);
Console.WriteLine("5. Gde je rabat manji ili veci od X% u stavkama izlaznog dokumenta");
Console.WriteLine("0. Izadji");
Console.Write("Izaberi opciju: ");

var source = new DB(
	$"data source=4monitor; initial catalog = {bazaConnString}; user=SYSDBA; password=m"
);
var ctx = source.CreateContext();
var sifarnik = ctx.Roba.ToDictionary(x => x.Id, x => x);
string opt = string.Empty;
while (string.IsNullOrWhiteSpace(opt))
{
	opt = Console.ReadLine();
	switch (opt)
	{
		case "1":
			IzlistajOsnovnaNeslaganja();
			break;
		case "2":
			IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaProdajnaCena();
			break;
		case "3":
			IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaNabavnaCena();
			break;
		case "4":
			IzlistajStavkeSaKolicinomIAbsolutnomRazlikomProdajneINabavneCene();
			break;
		case "5":
			IzlistajStavkeSaKolicinomIAbsolutnimRabatomOd();
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

void PrintHeader(Dokument sourceDok, Dokument destinacioniDokument)
{
	Console.WriteLine();
	Console.WriteLine("===");
	Console.WriteLine($"Izvorni VrDok: {sourceDok.VrDok}");
	Console.WriteLine($"Izvorni BrDok: {sourceDok.BrDok}");
	Console.WriteLine($"Izvorni MagacinId: {sourceDok.MagacinId}");
	Console.WriteLine($"Destinacioni VrDok: {destinacioniDokument.VrDok}");
	Console.WriteLine($"Destinacioni BrDok: {destinacioniDokument.BrDok}");
	Console.WriteLine($"Destinacioni MagacinId: {destinacioniDokument.MagacinId}");
}
void PrintFooter()
{
	Console.WriteLine("===");
}
void PrintRoba(int robaId) => Console.WriteLine(FormatRobaPrint(robaId));
string FormatRobaPrint(int robaId)
{
	var sb = new StringBuilder();
	sb.AppendLine();
    var roba = sifarnik[robaId];
    sb.AppendLine($"[{roba.KatBr}] {roba.Naziv}");
	return sb.ToString();
}
Tuple<int, int>? IzborDokumenta()
{
	Console.WriteLine();
	Console.WriteLine("Radi proveru na osnovu dokumenata internog transporta: ");
	Console.WriteLine("1. Odredjenog dokumenta");
	Console.WriteLine("2. Svih dokumenata");
	opt = string.Empty;
	while (string.IsNullOrWhiteSpace(opt))
	{
		opt = Console.ReadLine();
		switch (opt)
		{
			case "1":
				int vrDok;
				int brDok;
				Console.Write("Unesi vrstu dokumenta: ");
				while (!int.TryParse(Console.ReadLine(), out vrDok))
				{
					Console.WriteLine("Neispravna vrsta dokumenta!");
					Console.Write("Unesi vrstu dokumenta: ");
				}
				Console.Write("Unesi broj dokumenta: ");
				while (!int.TryParse(Console.ReadLine(), out brDok))
				{
					Console.WriteLine("Neispravna vrsta dokumenta!");
					Console.Write("Unesi broj dokumenta: ");
				}
				return new Tuple<int, int>(vrDok, brDok);
			case "2":
				return null;
			default:
				Console.WriteLine("Nepoznata opcija!");
				break;
		}
	}
	throw new Exception("ne bi trebao da si ovde!");
}

void IzlistajOsnovnaNeslaganja()
{
	var dok = IzborDokumenta();
	if (dok == null)
		IzlistajOsnovnaNeslaganjaSvihDokumenata();
	else
		IzlistajNeslaganjaDokumenta(
			dok!,
			true,
			new WhatToCheckFilter
			{
				BrojStavki = true,
				Kolicine = true,
				SveStavkeIste = true,
				ProdajnaCenaNula = false,
				NabavnaCenaNula = false,
			}
		);
}

void IzlistajOsnovnaNeslaganjaSvihDokumenata()
{
	var dokumentiSaInternimTransportom = ctx
		.InterniTransporti.Select(x => new Tuple<int, int>(x.IzVrDok, x.IzBrDok))
		.Distinct();

	foreach (var dok in dokumentiSaInternimTransportom)
		IzlistajNeslaganjaDokumenta(
			dok,
			false,
			new WhatToCheckFilter
			{
				BrojStavki = true,
				Kolicine = true,
				SveStavkeIste = true,
				ProdajnaCenaNula = false,
				NabavnaCenaNula = false,
			}
		);
}

bool IsStandardRoba(int robaId) => sifarnik.Keys.Contains(robaId) && sifarnik[robaId].Vrsta == 1;
void IzlistajNeslaganjaDokumenta(
	Tuple<int, int> dok,
	bool alertNotTransported,
	WhatToCheckFilter whatToCheck
)
{
	if (whatToCheck is { SveStavkeIste: false, Kolicine: true })
		throw new Exception(
			"Ako se proveravaju kolicine, onda moraju da se proveravaju i da su sve stavke iste!"
		);
	if (whatToCheck is { SveStavkeIste: false, ProdajnaCenaNula: true })
		throw new Exception(
			"Ako se proveravaju prodajne cene, onda moraju da se proveravaju i da su sve stavke iste!"
		);
	if (whatToCheck is { SveStavkeIste: false, NabavnaCenaNula: true })
		throw new Exception(
			"Ako se proveravaju nabavne cene, onda moraju da se proveravaju i da su sve stavke iste!"
		);
	var vrDok = dok.Item1;
	var brDok = dok.Item2;
	var dokument = ctx
		.Dokumenti.Where(x => x.VrDok == vrDok && x.BrDok == brDok)
		.Include(x => x.Stavke)
		.FirstOrDefault();
	if (dokument == null)
	{
		Console.WriteLine();
		Console.WriteLine("Dokument nije pronadjen!");
		return;
	}

	var interniTransporti = ctx
		.InterniTransporti.Where(x => x.IzVrDok == vrDok && x.IzBrDok == brDok)
		.ToList();
	if (!interniTransporti.Any())
	{
		if (alertNotTransported)
		{
			Console.WriteLine();
			Console.WriteLine(
				"Ovaj dokument nije interno transportovan niti u jedan drugi dokument!"
			);
		}
		return;
	}

	var vrstaRoba = 1;
	var originalnoBrojStavki = dokument.Stavke.Count(x => IsStandardRoba(x.RobaId));
	foreach (var interniTransport in interniTransporti)
	{
		var printedHeader = false;
		var dokumentTransporta = ctx
			.Dokumenti.Where(x =>
				x.VrDok == interniTransport.UVrDok && x.BrDok == interniTransport.UBrDok
			)
			.Include(x => x.Stavke)
			.FirstOrDefault();
		if (dokumentTransporta == null)
			throw new NullReferenceException(nameof(dokumentTransporta));

		// check broj stavki
		if (whatToCheck.BrojStavki)
		{
			var transportovanoBrojStavki = dokumentTransporta.Stavke.Count(x =>
				IsStandardRoba(x.RobaId)
			);
			if (originalnoBrojStavki != transportovanoBrojStavki)
			{
				if (!printedHeader)
				{
					PrintHeader(dokument, dokumentTransporta);
					printedHeader = true;
				}
				Console.WriteLine("Neslaganje u broju stavki!");
				Console.WriteLine($"Izvorni dokument stavki: {originalnoBrojStavki}");
				Console.WriteLine($"Transportovani dokument stavki: {transportovanoBrojStavki}");
			}
		}

		// check sve stavke iste
		bool sveStavkeIste = false;
		if (whatToCheck.SveStavkeIste)
		{
			var distinctRobaIdsUTransportovanomDokumentu = dokumentTransporta
				.Stavke.Where(x => IsStandardRoba(x.RobaId))
				.Select(x => x.RobaId)
				.Distinct()
				.ToHashSet();
			var sveStavkeIsteListCheck = dokument
				.Stavke.Where(x => IsStandardRoba(x.RobaId))
				.ToList();
			sveStavkeIsteListCheck.RemoveAll(x =>
				distinctRobaIdsUTransportovanomDokumentu.Contains(x.RobaId)
			);
			sveStavkeIste = sveStavkeIsteListCheck.Count == 0;
			if (!sveStavkeIste)
			{
				if (!printedHeader)
				{
					PrintHeader(dokument, dokumentTransporta);
					printedHeader = true;
				}
				Console.WriteLine("Neslaganje u samim stavkama!");
				Console.WriteLine(
					"U izvornom dokumentu postoje stavke koje ne postoje u destinacionom ili obratno!"
				);
				foreach (var st in sveStavkeIsteListCheck)
					PrintRoba(st.RobaId);
			}
		}

		// check kolicine
		if (whatToCheck.Kolicine)
		{
			if (sveStavkeIste)
			{
				foreach (var stavka in dokument.Stavke.Where(x => IsStandardRoba(x.RobaId)))
				{
					var stavkaDestinacionogDokumenta = dokumentTransporta
						.Stavke.Where(x => IsStandardRoba(x.RobaId))
						.First(x => x.RobaId == stavka.RobaId);
					if (Math.Abs(stavkaDestinacionogDokumenta.Kolicina - stavka.Kolicina) > 0.01)
					{
						if (!printedHeader)
						{
							PrintHeader(dokument, dokumentTransporta);
							printedHeader = true;
						}
						Console.WriteLine(
							"Neslaganje u kolicinama stavke izvornog i destinacionog dokumenta!"
						);
						PrintRoba(stavkaDestinacionogDokumenta.RobaId);
						Console.WriteLine($"Izvorna kolicina: {stavka.Kolicina:#,##0.00}");
						Console.WriteLine(
							$"Destinaciona kolicina: {stavkaDestinacionogDokumenta.Kolicina:#,##0.00}"
						);
					}
				}
			}
		}

		// check prodajne cene
		if (whatToCheck.ProdajnaCenaNula)
		{
			if (sveStavkeIste)
			{
				foreach (var stavka in dokument.Stavke.Where(x => IsStandardRoba(x.RobaId)))
				{
					var stavkaDestinacionogDokumenta = dokumentTransporta
						.Stavke.Where(x => IsStandardRoba(x.RobaId))
						.First(x => x.RobaId == stavka.RobaId);
					if (
						Math.Abs(stavkaDestinacionogDokumenta.ProdajnaCena - stavka.ProdajnaCena)
						> 0.01
					)
					{
						if (!printedHeader)
						{
							PrintHeader(dokument, dokumentTransporta);
							printedHeader = true;
						}
						Console.WriteLine(
							"Neslaganje u prodajnim cenama stavke izvornog i destinacionog dokumenta!"
						);
						PrintRoba(stavkaDestinacionogDokumenta.RobaId);
						Console.WriteLine($"Izvorna prodajna cena: {stavka.ProdajnaCena:#,##0.00}");
						Console.WriteLine(
							$"Destinaciona prodajna cena: {stavkaDestinacionogDokumenta.ProdajnaCena:#,##0.00}"
						);
					}
				}
			}
		}

		// check nabavne cene
		if (whatToCheck.NabavnaCenaNula)
		{
			if (sveStavkeIste)
			{
				foreach (var stavka in dokument.Stavke.Where(x => IsStandardRoba(x.RobaId)))
				{
					var stavkaDestinacionogDokumenta = dokumentTransporta
						.Stavke.Where(x => IsStandardRoba(x.RobaId))
						.First(x => x.RobaId == stavka.RobaId);
					if (
						Math.Abs(stavkaDestinacionogDokumenta.NabavnaCena - stavka.NabavnaCena)
						> 0.01
					)
					{
						if (!printedHeader)
						{
							PrintHeader(dokument, dokumentTransporta);
							printedHeader = true;
						}
						Console.WriteLine(
							"Neslaganje u nabavnim cenama stavke izvornog i destinacionog dokumenta!"
						);
						PrintRoba(stavkaDestinacionogDokumenta.RobaId);
						Console.WriteLine($"Izvorna nabavna cena: {stavka.NabavnaCena:#,##0.00}");
						Console.WriteLine(
							$"Destinaciona nabavna cena: {stavkaDestinacionogDokumenta.NabavnaCena:#,##0.00}"
						);
					}
				}
			}
		}
		if (printedHeader)
			PrintFooter();
	}
}

void IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaProdajnaCena()
{
	var dok = IzborDokumenta();
	if (dok == null)
		IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaProdajnaCenaSvihDokumenata();
	else
		IzlistajNeslaganjaDokumenta(
			dok!,
			true,
			new WhatToCheckFilter
			{
				BrojStavki = true,
				Kolicine = false,
				SveStavkeIste = true,
				ProdajnaCenaNula = true,
				NabavnaCenaNula = false,
			}
		);
}

void IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaProdajnaCenaSvihDokumenata()
{
	var dokumentiSaInternimTransportom = ctx
		.InterniTransporti.Select(x => new Tuple<int, int>(x.IzVrDok, x.IzBrDok))
		.Distinct();

	foreach (var dok in dokumentiSaInternimTransportom)
		IzlistajNeslaganjaDokumenta(
			dok,
			false,
			new WhatToCheckFilter
			{
				BrojStavki = true,
				Kolicine = false,
				SveStavkeIste = true,
				ProdajnaCenaNula = true,
				NabavnaCenaNula = false,
			}
		);
}

void IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaNabavnaCena()
{
	var dok = IzborDokumenta();
	if (dok == null)
		IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaNabavnaCenaSvihDokumenata();
	else
		IzlistajNeslaganjaDokumenta(
			dok!,
			true,
			new WhatToCheckFilter
			{
				BrojStavki = true,
				Kolicine = false,
				SveStavkeIste = true,
				ProdajnaCenaNula = false,
				NabavnaCenaNula = true,
			}
		);
}

void IzlistajNeslaganjaGdeUIzlaznomDokumentuNemaNabavnaCenaSvihDokumenata()
{
	var dokumentiSaInternimTransportom = ctx
		.InterniTransporti.Select(x => new Tuple<int, int>(x.IzVrDok, x.IzBrDok))
		.Distinct();

	foreach (var dok in dokumentiSaInternimTransportom)
		IzlistajNeslaganjaDokumenta(
			dok,
			false,
			new WhatToCheckFilter
			{
				BrojStavki = true,
				Kolicine = false,
				SveStavkeIste = true,
				ProdajnaCenaNula = false,
				NabavnaCenaNula = true,
			}
		);
}

int UnosZnaka()
{
	Console.WriteLine();
	Console.WriteLine("Zelim da selektuje stavke kojima je vrednost filtera: ");
	Console.WriteLine("1. Veci/a od X%");
	Console.WriteLine("2. Manji/a od X%");
	var opt = Console.ReadLine();
	int o = 0;
	if (!int.TryParse(opt, out o))
	{
		Console.WriteLine("Neispravna opcija!");
		UnosZnaka();
	}
	return o == 1 ? 1 : -1;
}
double UnosRazlikeProcenti()
{
	double razlikaProcenti;
	Console.WriteLine();
	Console.Write("Unesite procenat razlike vrednosti: ");
	var b = Console.ReadLine();
	if (!double.TryParse(b, out razlikaProcenti))
	{
		Console.WriteLine("Procenat nije dobar!");
		UnosRazlikeProcenti();
	}
	if (razlikaProcenti <= 0)
	{
		Console.WriteLine("Unesite procenat u pozitivnoj vrednosti.");
		UnosRazlikeProcenti();
	}
	return razlikaProcenti;
}
void IzlistajStavkeSaKolicinomIAbsolutnomRazlikomProdajneINabavneCene()
{
    Console.WriteLine();
    Console.WriteLine(
        "Ova akcija ce selektovati i ispisati sve stavke destinacionih dokumenata internog trasporta kojima je razlika izmedju prodajne i nabavne cene veca ili manja od X%"
    );
    var dok = IzborDokumenta();
	var znak = UnosZnaka();
	var razlikaProcenti = UnosRazlikeProcenti();

	if (dok == null)
		IzlistajStavkeSaKolicinomIAbsolutnomRazlikomProdajneINabavneCeneSviDokumenti(
			znak,
			razlikaProcenti
		);
	else
		IzlistajStavkeSaKolicinomIAbsolutnomRazlikomProdajneINabavneCeneDokumenta(
			ctx.Dokumenti.First(x => x.VrDok == dok.Item1 && x.BrDok == dok.Item2),
			znak,
			razlikaProcenti
		);
}
void IzlistajStavkeSaKolicinomIAbsolutnomRazlikomProdajneINabavneCeneSviDokumenti(
	int znak,
	double procenti
)
{
	var dokumentiSaInternimTransportom = ctx
		.InterniTransporti.Select(x => new Tuple<int, int>(x.IzVrDok, x.IzBrDok))
		.Distinct();

	foreach (var d in dokumentiSaInternimTransportom)
	{
		var dd = ctx.Dokumenti.FirstOrDefault(x => x.VrDok == d.Item1 && x.BrDok == d.Item2);
		if (dd == null)
		{
			Console.WriteLine($"Nasao sam neki invalidan interni transport [IzVrDok: {d.Item1}, IzBrDok: {d.Item2}]");
			continue;
		}
		IzlistajStavkeSaKolicinomIAbsolutnomRazlikomProdajneINabavneCeneDokumenta(
			dd,
			znak,
			procenti
		);
	}
}
void IzlistajStavkeSaKolicinomIAbsolutnomRazlikomProdajneINabavneCeneDokumenta(
	Dokument d,
	int znak,
	double razlikaProcenti
)
{

	var transporti = ctx.InterniTransporti.Where(x => x.IzVrDok == d.VrDok && x.IzBrDok == d.BrDok);
	foreach (var t in transporti)
	{
		var dok = ctx.Dokumenti.First(x => x.VrDok == t.UVrDok && x.BrDok == t.UBrDok);
		var stavke = ctx
			.Stavke.Where(x => x.VrDok == dok.VrDok && x.BrDok == dok.BrDok && x.Kolicina != 0)
			.ToList();
		var sb = new StringBuilder();
        foreach (var stavka in stavke)
		{
			if (stavka.ProdajnaCena == 0)
			{
				sb.AppendLine(FormatRobaPrint(stavka.RobaId));
				sb.AppendLine($"Prodajna cena 0!");
				continue;
			}
			if (stavka.NabavnaCena == 0)
            {
                sb.AppendLine(FormatRobaPrint(stavka.RobaId));
                sb.AppendLine($"Nabavna cena 0!");
                continue;
			}
			if (stavka.ProdajnaCena <= stavka.NabavnaCena)
            {
                sb.AppendLine(FormatRobaPrint(stavka.RobaId));
                sb.AppendLine($"Prodajna cena == Nabavna cena!");
				continue;
			}
			var razlikaPerc = (stavka.ProdajnaCena / stavka.NabavnaCena - 1) * 100;
			if (znak == 1)
			{
				if (razlikaPerc > razlikaProcenti)
				{
                    sb.AppendLine(FormatRobaPrint(stavka.RobaId));
                    sb.AppendLine($"Razlika prodajne i nabavne cene veca od {razlikaProcenti}%!");
                    sb.AppendLine($"Nabavna cena: {stavka.NabavnaCena}");
                    sb.AppendLine($"Prodajna cena: {stavka.ProdajnaCena}");
                    continue;
				}
			}
			else
			{
				if (razlikaPerc < razlikaProcenti)
                {
                    sb.AppendLine(FormatRobaPrint(stavka.RobaId));
                    sb.AppendLine($"Razlika prodajne i nabavne cene manja od {razlikaProcenti}%!");
                    sb.AppendLine($"Nabavna cena: {stavka.NabavnaCena}");
                    sb.AppendLine($"Prodajna cena: {stavka.ProdajnaCena}");
                    continue;
				}
			}
		}
		if(sb.Length > 0)
		{
			PrintHeader(d, dok);
			Console.WriteLine(sb.ToString());
			PrintFooter();
		}
	}
}

void IzlistajStavkeSaKolicinomIAbsolutnimRabatomOd()
{
    Console.WriteLine();
    Console.WriteLine(
        $"Ova akcija ce selektovati i ispisati sve stavke destinacionih dokumenata internog trasporta kojima je rabat veci/manji od X%"
    );
    var dok = IzborDokumenta();
	var znak = UnosZnaka();
	var razlikaProcenti = UnosRazlikeProcenti();

	if (dok == null)
		IzlistajStavkeSaKolicinomIAbsolutnimRabatomOdSviDokumenti(znak, razlikaProcenti);
	else
		IzlistajStavkeSaKolicinomIAbsolutnimRabatomOdDokumenta(
			ctx.Dokumenti.First(x => x.VrDok == dok.Item1 && x.BrDok == dok.Item2),
			znak,
			razlikaProcenti
		);
}
void IzlistajStavkeSaKolicinomIAbsolutnimRabatomOdSviDokumenti(int znak, double procenti)
{
	var dokumentiSaInternimTransportom = ctx
		.InterniTransporti.Select(x => new Tuple<int, int>(x.IzVrDok, x.IzBrDok))
		.Distinct();

	foreach (var d in dokumentiSaInternimTransportom)
	{
		var dd = ctx.Dokumenti.FirstOrDefault(x => x.VrDok == d.Item1 && x.BrDok == d.Item2);
		if(dd == null)
		{
            Console.WriteLine($"Nasao sam neki invalidan interni transport [IzVrDok: {d.Item1}, IzBrDok: {d.Item2}]");
			continue;
		}
        IzlistajStavkeSaKolicinomIAbsolutnimRabatomOdDokumenta(
			dd,
			znak,
			procenti
		);
	}
}
void IzlistajStavkeSaKolicinomIAbsolutnimRabatomOdDokumenta(
	Dokument d,
	int znak,
	double razlikaProcenti
)
{
	var transporti = ctx.InterniTransporti.Where(x => x.IzVrDok == d.VrDok && x.IzBrDok == d.BrDok);
	foreach (var t in transporti)
	{
		var dok = ctx.Dokumenti.First(x => x.VrDok == t.UVrDok && x.BrDok == t.UBrDok);
		var stavke = ctx
			.Stavke.Where(x => x.VrDok == dok.VrDok && x.BrDok == dok.BrDok && x.Kolicina != 0)
			.ToList();
		var sb = new StringBuilder();
		foreach (var stavka in stavke)
		{
			if (znak == 1)
			{
				if (stavka.Rabat > razlikaProcenti)
				{
					sb.AppendLine(FormatRobaPrint(stavka.RobaId));
					sb.AppendLine($"Rabat je veci od {razlikaProcenti}%!");
					sb.AppendLine($"Rabat: {stavka.Rabat}");
					continue;
				}
			}
			else
			{
				if (stavka.Rabat < razlikaProcenti)
                {
                    sb.AppendLine(FormatRobaPrint(stavka.RobaId));
                    sb.AppendLine($"Rabat je manji od {razlikaProcenti}%!");
                    sb.AppendLine($"Rabat: {stavka.Rabat}");
                    continue;
				}
			}
		}
		if (sb.Length > 0)
		{
			PrintHeader(d, dok);
			Console.WriteLine(sb.ToString());
			PrintFooter();
		}
    }
}
