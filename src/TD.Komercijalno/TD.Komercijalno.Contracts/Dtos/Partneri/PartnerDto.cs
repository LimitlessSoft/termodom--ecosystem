namespace TD.Komercijalno.Contracts.Dtos.Partneri;

public class PartnerDto
{
	public int Ppid { get; set; }
	public string Naziv { get; set; }
	public string? Adresa { get; set; }
	public string? Posta { get; set; }
	public string? Mesto { get; set; }
	public string? Telefon { get; set; }
	public string? Fax { get; set; }
	public string? Email { get; set; }
	public string? Kontakt { get; set; }
	public long? Kategorija { get; set; }
	public string MestoId { get; set; }
	public short? ZapId { get; set; }
	public short? RefId { get; set; }
	public string? Pib { get; set; }
	public string? Mobilni { get; set; }
	public string NazivZaStampu { get; set; }
	public short? Aktivan { get; set; }
}
