using TD.Komercijalno.Client;

namespace TD.Office.Public.Contracts.Dtos.KomercijalnoMagacinFirma;

public class GetKomercijalnoMagacinFirmaDto
{
	public long MagacinId { get; set; }
	public TDKomercijalnoFirma ApiFirma { get; set; }
}
