namespace TD.Komercijalno.Contracts.Dtos.IstorijaUplata;

public class IstorijaUplataDto
{
	public int Id { get; set; }
	public int VrDok { get; set; }
	public int BrDok { get; set; }
	public DateTime Datum { get; set; }
	public double Iznos { get; set; }
	public string? Opis { get; set; }
	public int PPID { get; set; }
	public short? ZapId { get; set; }
	public short? IO { get; set; }
	public short? NUID { get; set; }
	public short? A { get; set; }
	public short? KasaId { get; set; }
	public string? MTID { get; set; }
	public string Valuta { get; set; }
	public double Kurs { get; set; }
	public int? PromenaId { get; set; }
	public int PkId { get; set; }
	public int? LinkedId { get; set; }
	public int? PlacanjeId { get; set; }
	public short? Godina { get; set; }
	public int? PocStaId { get; set; }
}
