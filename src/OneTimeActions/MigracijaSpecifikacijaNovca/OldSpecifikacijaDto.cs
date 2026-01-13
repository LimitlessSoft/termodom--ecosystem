namespace MigracijaSpecifikacijaNovca;

public class OldSpecifikacijaDto
{
	public int Id { get; set; }
	public DateTime Datum { get; set; }
	public int MagacinId { get; set; }
	public OldSpecifikacijaDetails Detalji { get; set; }
}

public class OldSpecifikacijaDetails
{
	public OldSpecifikacijaKurs Kurs1 { get; set; }
	public OldSpecifikacijaKurs Kurs2 { get; set; }
	public double Novcanice5000 { get; set; }
	public double Novcanice2000 { get; set; }
	public double Novcanice1000 { get; set; }
	public double Novcanice500 { get; set; }
	public double Novcanice200 { get; set; }
	public double Novcanice100 { get; set; }
	public double Novcanice50 { get; set; }
	public double Novcanice20 { get; set; }
	public double Novcanice10 { get; set; }
	public double Kartice { get; set; }
	public double Cekovi { get; set; }
	public double Papiri { get; set; }
	public double Troskovi { get; set; }
	public double Storno { get; set; }
	public string Beleksa { get; set; }
}

public class OldSpecifikacijaKurs
{
	public double Kolicina { get; set; }
	public double Kurs { get; set; }
}
