namespace TD.Komercijalno.Contracts.Dtos.Promene;

public class PromenaDto
{
	public int Id { get; set; }
	public short VrstaNaloga { get; set; }
	public string BrojNaloga { get; set; }
	public DateTime Datum { get; set; }
	public string Konto { get; set; }
	public string? Opis { get; set; }
	public int? PPID { get; set; }
	public int? BrDok { get; set; }
	public int? VrDok { get; set; }
	public DateTime? DATDPO { get; set; }
	public double? Duguje { get; set; }
	public double? Potrazuje { get; set; }
	public DateTime? DatumValute { get; set; }
	public double? VDuguje { get; set; }
	public double? VPotrazuje { get; set; }
	public string? Valuta { get; set; }
	public double Kurs { get; set; }
	public string? MTID { get; set; }
	public short? A { get; set; }
	public int? StavkaID { get; set; }
}
