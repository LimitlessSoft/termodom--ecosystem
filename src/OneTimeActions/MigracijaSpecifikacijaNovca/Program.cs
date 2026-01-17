using System.Text;
using FirebirdSql.Data.FirebirdClient;
using MigracijaSpecifikacijaNovca;
using Newtonsoft.Json;

var pathTdOfficeBaza = "C:\\Poslovanje\\Baze\\TDOffice_v2\\TDOffice_v2_2021.FDB";
Console.WriteLine("Zapocinjem migraciju specifikacije novca...");
using var fbConn = new FbConnection(
	$"data source=4monitor; initial catalog = {pathTdOfficeBaza}; user=SYSDBA; password=m"
);
fbConn.Open();
using var fbCommand = new FbCommand("SELECT * FROM SPECIFIKACIJA_NOVCA", fbConn);
using var fbDataReader = fbCommand.ExecuteReader();
var oldSpecs = new List<OldSpecifikacijaDto>();
while (fbDataReader.Read())
{
	var tag = Encoding.UTF8.GetString(((byte[])fbDataReader["Tag"]));

	oldSpecs.Add(
		new OldSpecifikacijaDto()
		{
			Datum = Convert.ToDateTime(fbDataReader["Datum"]),
			Id = Convert.ToInt32(fbDataReader["Id"]),
			MagacinId = Convert.ToInt32(fbDataReader["MagacinId"]),
			Detalji = JsonConvert.DeserializeObject<OldSpecifikacijaDetails>(tag),
		}
	);
}
//PrintSpecifikacijes(oldSpecs);

// PostgreSQL connection string - update these values for your environment
var pgConnectionString =
	"Server=139.177.181.216;Port=5432;Userid=postgres;Password=FFnF2JegHu0pt6RmBr5ib2mxRIua55555;Database=production_tdoffice;";
ValidateNoSameSpecificationInOldAndNew(pgConnectionString, oldSpecs);

// PrintSpecifikacijesFromNewDb(pgConnectionString);
MigrateSpecifikacijes(oldSpecs, pgConnectionString);

void ValidateNoSameSpecificationInOldAndNew(
	string connectionString,
	List<OldSpecifikacijaDto> oldOnes
)
{
	Console.WriteLine();
	Console.WriteLine("Validacija duplikata...");

	using var dbContext = new MigrationDbContext(connectionString);
	var existingSpecs = dbContext
		.SpecifikacijeNovca.Select(x => new { x.MagacinId, Date = x.Datum.Date })
		.ToHashSet();

	var duplicates = oldOnes
		.Where(old =>
			existingSpecs.Contains(new { MagacinId = (long)old.MagacinId, Date = old.Datum.Date })
		)
		.ToList();

	if (duplicates.Count > 0)
	{
		Console.WriteLine($"GREŠKA: Pronađeno {duplicates.Count} duplikata!");
		Console.WriteLine(new string('=', 80));
		foreach (var dup in duplicates)
		{
			Console.WriteLine($"  MagacinId: {dup.MagacinId} | Datum: {dup.Datum:dd.MM.yyyy}");
		}
		Console.WriteLine(new string('=', 80));
		throw new Exception(
			$"Migracija prekinuta: {duplicates.Count} specifikacija već postoji u novoj bazi."
		);
	}

	Console.WriteLine("Validacija uspešna - nema duplikata.");
}
void MigrateSpecifikacijes(List<OldSpecifikacijaDto> list, string connectionString)
{
	Console.WriteLine();
	Console.WriteLine("Zapocinjem migraciju u PostgreSQL bazu...");

	using var dbContext = new MigrationDbContext(connectionString);

	var entitiesToInsert = new List<TD.Office.Common.Contracts.Entities.SpecifikacijaNovcaEntity>();

	foreach (var old in list)
	{
		var entity = new TD.Office.Common.Contracts.Entities.SpecifikacijaNovcaEntity
		{
			MagacinId = old.MagacinId,
			Datum = DateTime.SpecifyKind(old.Datum, DateTimeKind.Utc),
			IsActive = true,
			CreatedBy = 0, // System/migration user
		};

		if (old.Detalji != null)
		{
			entity.Komentar = old.Detalji.Beleksa;

			// EUR kursevi
			if (old.Detalji.Kurs1 != null)
			{
				entity.Eur1Komada = (int)old.Detalji.Kurs1.Kolicina;
				entity.Eur1Kurs = old.Detalji.Kurs1.Kurs;
			}
			if (old.Detalji.Kurs2 != null)
			{
				entity.Eur2Komada = (int)old.Detalji.Kurs2.Kolicina;
				entity.Eur2Kurs = old.Detalji.Kurs2.Kurs;
			}

			// Novčanice
			entity.Novcanica5000Komada = (int)old.Detalji.Novcanice5000;
			entity.Novcanica2000Komada = (int)old.Detalji.Novcanice2000;
			entity.Novcanica1000Komada = (int)old.Detalji.Novcanice1000;
			entity.Novcanica500Komada = (int)old.Detalji.Novcanice500;
			entity.Novcanica200Komada = (int)old.Detalji.Novcanice200;
			entity.Novcanica100Komada = (int)old.Detalji.Novcanice100;
			entity.Novcanica50Komada = (int)old.Detalji.Novcanice50;
			entity.Novcanica20Komada = (int)old.Detalji.Novcanice20;
			entity.Novcanica10Komada = (int)old.Detalji.Novcanice10;

			// Ostalo
			entity.Kartice = old.Detalji.Kartice;
			entity.Cekovi = old.Detalji.Cekovi;
			entity.Papiri = old.Detalji.Papiri;
			entity.Troskovi = old.Detalji.Troskovi;
			entity.Sasa = old.Detalji.KodSase;
			entity.Vozaci = old.Detalji.VozaciDuguju;

			entity.KarticeKomentar = old.Detalji.KarticeBeleksa;
			entity.CekoviKomentar = old.Detalji.CekoviBeleksa;
			entity.PapiriKomentar = old.Detalji.PapiriBeleksa;
			entity.SasaKomentar = old.Detalji.KodSaseBeleksa;
			entity.TroskoviKomentar = old.Detalji.TroskoviBeleksa;
			entity.VozaciKomentar = old.Detalji.VozaciDugujuBeleksa;

        }

		entitiesToInsert.Add(entity);
	}

	Console.WriteLine($"Pripremljeno {entitiesToInsert.Count} entiteta za unos.");

	dbContext.SpecifikacijeNovca.AddRange(entitiesToInsert);
	var saved = dbContext.SaveChanges();

	Console.WriteLine($"Uspešno migrirano {saved} specifikacija novca.");
}

void PrintSpecifikacijes(List<OldSpecifikacijaDto> list)
{
	Console.WriteLine($"Ukupno specifikacija: {list.Count}");
	Console.WriteLine(new string('=', 80));

	foreach (var spec in list)
	{
		Console.WriteLine(
			$"ID: {spec.Id} | Datum: {spec.Datum:dd.MM.yyyy} | Magacin ID: {spec.MagacinId}"
		);

		if (spec.Detalji != null)
		{
			Console.WriteLine("  Novčanice:");
			Console.WriteLine($"    5000: {spec.Detalji.Novcanice5000, 10:N2}");
			Console.WriteLine($"    2000: {spec.Detalji.Novcanice2000, 10:N2}");
			Console.WriteLine($"    1000: {spec.Detalji.Novcanice1000, 10:N2}");
			Console.WriteLine($"     500: {spec.Detalji.Novcanice500, 10:N2}");
			Console.WriteLine($"     200: {spec.Detalji.Novcanice200, 10:N2}");
			Console.WriteLine($"     100: {spec.Detalji.Novcanice100, 10:N2}");
			Console.WriteLine($"      50: {spec.Detalji.Novcanice50, 10:N2}");
			Console.WriteLine($"      20: {spec.Detalji.Novcanice20, 10:N2}");
			Console.WriteLine($"      10: {spec.Detalji.Novcanice10, 10:N2}");

			Console.WriteLine("  Ostalo:");
			Console.WriteLine($"    Kartice:  {spec.Detalji.Kartice, 10:N2}");
			Console.WriteLine($"    Čekovi:   {spec.Detalji.Cekovi, 10:N2}");
			Console.WriteLine($"    Papiri:   {spec.Detalji.Papiri, 10:N2}");
			Console.WriteLine($"    Troškovi: {spec.Detalji.Troskovi, 10:N2}");
			Console.WriteLine($"    Storno:   {spec.Detalji.Storno, 10:N2}");

			if (spec.Detalji.Kurs1 != null)
				Console.WriteLine(
					$"  Kurs 1: Količina={spec.Detalji.Kurs1.Kolicina:N2}, Kurs={spec.Detalji.Kurs1.Kurs:N4}"
				);

			if (spec.Detalji.Kurs2 != null)
				Console.WriteLine(
					$"  Kurs 2: Količina={spec.Detalji.Kurs2.Kolicina:N2}, Kurs={spec.Detalji.Kurs2.Kurs:N4}"
				);

			if (!string.IsNullOrEmpty(spec.Detalji.Beleksa))
				Console.WriteLine($"  Beleška: {spec.Detalji.Beleksa}");
		}

		Console.WriteLine(new string('-', 80));
	}
}

void PrintSpecifikacijesFromNewDb(string connectionString)
{
	Console.WriteLine();
	Console.WriteLine("Čitam specifikacije novca iz PostgreSQL baze...");
	Console.WriteLine(new string('=', 80));

	using var dbContext = new MigrationDbContext(connectionString);
	var specs = dbContext.SpecifikacijeNovca.OrderBy(x => x.Datum).ToList();

	Console.WriteLine($"Ukupno specifikacija u novoj bazi: {specs.Count}");
	Console.WriteLine(new string('=', 80));

	foreach (var spec in specs)
	{
		Console.WriteLine(
			$"ID: {spec.Id} | Datum: {spec.Datum:dd.MM.yyyy} | Magacin ID: {spec.MagacinId} | Active: {spec.IsActive}"
		);

		Console.WriteLine("  Novčanice:");
		Console.WriteLine($"    5000: {spec.Novcanica5000Komada, 10} kom");
		Console.WriteLine($"    2000: {spec.Novcanica2000Komada, 10} kom");
		Console.WriteLine($"    1000: {spec.Novcanica1000Komada, 10} kom");
		Console.WriteLine($"     500: {spec.Novcanica500Komada, 10} kom");
		Console.WriteLine($"     200: {spec.Novcanica200Komada, 10} kom");
		Console.WriteLine($"     100: {spec.Novcanica100Komada, 10} kom");
		Console.WriteLine($"      50: {spec.Novcanica50Komada, 10} kom");
		Console.WriteLine($"      20: {spec.Novcanica20Komada, 10} kom");
		Console.WriteLine($"      10: {spec.Novcanica10Komada, 10} kom");
		Console.WriteLine($"       5: {spec.Novcanica5Komada, 10} kom");
		Console.WriteLine($"       2: {spec.Novcanica2Komada, 10} kom");
		Console.WriteLine($"       1: {spec.Novcanica1Komada, 10} kom");

		Console.WriteLine("  Ostalo:");
		Console.WriteLine($"    Kartice:  {spec.Kartice, 10:N2}");
		Console.WriteLine($"    Čekovi:   {spec.Cekovi, 10:N2}");
		Console.WriteLine($"    Papiri:   {spec.Papiri, 10:N2}");
		Console.WriteLine($"    Troškovi: {spec.Troskovi, 10:N2}");
		Console.WriteLine($"    Vozači:   {spec.Vozaci, 10:N2}");
		Console.WriteLine($"    Saša:     {spec.Sasa, 10:N2}");

		if (spec.Eur1Komada > 0 || spec.Eur1Kurs > 0)
			Console.WriteLine($"  EUR 1: Komada={spec.Eur1Komada}, Kurs={spec.Eur1Kurs:N4}");

		if (spec.Eur2Komada > 0 || spec.Eur2Kurs > 0)
			Console.WriteLine($"  EUR 2: Komada={spec.Eur2Komada}, Kurs={spec.Eur2Kurs:N4}");

		if (!string.IsNullOrEmpty(spec.Komentar))
			Console.WriteLine($"  Komentar: {spec.Komentar}");

		Console.WriteLine(new string('-', 80));
	}
}
