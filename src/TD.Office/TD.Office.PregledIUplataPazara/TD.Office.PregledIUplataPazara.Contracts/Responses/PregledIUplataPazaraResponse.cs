namespace TD.Office.PregledIUplataPazara.Contracts.Responses;

public class PregledIUplataPazaraResponse
{
	public double UkupnaRazlika { get; set; }
	public string UkupnaRazlikaFormatted => UkupnaRazlika.ToString("#,##0.00 RSD");
	public double UkupnoMpRacuna { get; set; }
	public string UkupnoMpRacunaFormatted => UkupnoMpRacuna.ToString("#,##0.00 RSD");
	public double UkupnoPovratnice { get; set; }
	public string UkupnoPovratniceFormatted => UkupnoPovratnice.ToString("#,##0.00 RSD");
	public double UkupnoPotrazuje { get; set; }
	public string UkupnoPotrazujeFormatted => UkupnoPotrazuje.ToString("#,##0.00 RSD");
	public List<PregledIUplataPazaraResponseItem> Items { get; set; } = [];
}

public class PregledIUplataPazaraResponseItem
{
	public string Konto { get; set; }
	public string PozNaBroj { get; set; }
	public int MagacinId { get; set; }
	public DateTime Datum { get; set; }
	public double MPRacuni { get; set; }
	public double Povratnice { get; set; }
	public double ZaUplatu => MPRacuni - Povratnice;
	public double Potrazuje { get; set; }
	public double Razlika => Potrazuje - ZaUplatu;
	public List<PregledIUplataPazaraResponseItemIzvodDto> Izvodi { get; set; } = [];
}

public class PregledIUplataPazaraResponseItemIzvodDto
{
	public int VrDok { get; set; }
	public int BrojIzvoda { get; set; }
	public string ZiroRacun { get; set; }
	public string Konto { get; set; }
	public string PozivNaBroj { get; set; }
	public int MagacinId { get; set; }
	public double Potrazuje { get; set; }
	public double Duguje { get; set; }
}
