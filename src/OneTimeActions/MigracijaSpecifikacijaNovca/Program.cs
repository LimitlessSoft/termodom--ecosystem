using FirebirdSql.Data.FirebirdClient;
using MigracijaSpecifikacijaNovca;
using Newtonsoft.Json;

var pathTdOfficeBaza = "C:\\Poslovanje\\Baze\\TDOffice_v2\\TDOffice_v2_2021.fdb";
Console.WriteLine("Zapocinjem migraciju specifikacije novca...");
using var fbConn = new FbConnection(pathTdOfficeBaza);
fbConn.Open();
using var fbCommand = new FbCommand("SELECT * FROM SPECIFIKACIJA_NOVCA", fbConn);
using var fbDataReader = fbCommand.ExecuteReader();
var oldSpecs = new List<OldSpecifikacijaDto>();
while (fbDataReader.Read())
{
	oldSpecs.Add(
		new OldSpecifikacijaDto()
		{
			Datum = Convert.ToDateTime(fbDataReader["Datum"]),
			Id = Convert.ToInt32(fbDataReader["Id"]),
			MagacinId = Convert.ToInt32(fbDataReader["MagacinId"]),
			Detalji = JsonConvert.DeserializeObject<OldSpecifikacijaDetails>(
				fbDataReader["Tag"].ToString()
			),
		}
	);
}
PrintSpecifikacijes(oldSpecs);

void PrintSpecifikacijes(List<OldSpecifikacijaDto> list)
{
	Console.WriteLine($"Ukupno specifikacija: {list.Count}");
	Console.WriteLine(new string('=', 80));

	foreach (var spec in list)
	{
		Console.WriteLine($"ID: {spec.Id} | Datum: {spec.Datum:dd.MM.yyyy} | Magacin ID: {spec.MagacinId}");

		if (spec.Detalji != null)
		{
			Console.WriteLine("  Novčanice:");
			Console.WriteLine($"    5000: {spec.Detalji.Novcanice5000,10:N2}");
			Console.WriteLine($"    2000: {spec.Detalji.Novcanice2000,10:N2}");
			Console.WriteLine($"    1000: {spec.Detalji.Novcanice1000,10:N2}");
			Console.WriteLine($"     500: {spec.Detalji.Novcanice500,10:N2}");
			Console.WriteLine($"     200: {spec.Detalji.Novcanice200,10:N2}");
			Console.WriteLine($"     100: {spec.Detalji.Novcanice100,10:N2}");
			Console.WriteLine($"      50: {spec.Detalji.Novcanice50,10:N2}");
			Console.WriteLine($"      20: {spec.Detalji.Novcanice20,10:N2}");
			Console.WriteLine($"      10: {spec.Detalji.Novcanice10,10:N2}");

			Console.WriteLine("  Ostalo:");
			Console.WriteLine($"    Kartice:  {spec.Detalji.Kartice,10:N2}");
			Console.WriteLine($"    Čekovi:   {spec.Detalji.Cekovi,10:N2}");
			Console.WriteLine($"    Papiri:   {spec.Detalji.Papiri,10:N2}");
			Console.WriteLine($"    Troškovi: {spec.Detalji.Troskovi,10:N2}");
			Console.WriteLine($"    Storno:   {spec.Detalji.Storno,10:N2}");

			if (spec.Detalji.Kurs1 != null)
				Console.WriteLine($"  Kurs 1: Količina={spec.Detalji.Kurs1.Kolicina:N2}, Kurs={spec.Detalji.Kurs1.Kurs:N4}");

			if (spec.Detalji.Kurs2 != null)
				Console.WriteLine($"  Kurs 2: Količina={spec.Detalji.Kurs2.Kolicina:N2}, Kurs={spec.Detalji.Kurs2.Kurs:N4}");

			if (!string.IsNullOrEmpty(spec.Detalji.Beleksa))
				Console.WriteLine($"  Beleška: {spec.Detalji.Beleksa}");
		}

		Console.WriteLine(new string('-', 80));
	}
}
