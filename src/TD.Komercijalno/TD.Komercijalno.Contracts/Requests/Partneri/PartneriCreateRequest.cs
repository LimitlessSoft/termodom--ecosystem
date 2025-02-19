namespace TD.Komercijalno.Contracts.Requests.Partneri;

public class PartneriCreateRequest
{
    public string Naziv { get; set; }
    public string? Adresa { get; set; }
    public string? Posta { get; set; }
    public string? Mesto { get; set; }
    public string? Email { get; set; }
    public string? Kontakt { get; set; }
    public long? Kategorija { get; set; }
    public string Mbroj { get; set; }
    public string MestoId { get; set; }
    public short? ZapId { get; set; }
    public short? RefId { get; set; }
    public string Pib { get; set; }
    public string Mobilni { get; set; }
    public bool UPdvSistemu { get; set; }
    public string TipPartnera { get; set; }
}
