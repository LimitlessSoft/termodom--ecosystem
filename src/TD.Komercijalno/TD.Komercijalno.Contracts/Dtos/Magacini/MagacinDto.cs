using TD.Komercijalno.Contracts.Enums;

namespace TD.Komercijalno.Contracts.Dtos.Magacini;

public class MagacinDto
{
	public long MagacinId { get; set; }
	public string Naziv { get; set; }
	public string MtId { get; set; }
	public short VodiSe { get; set; }
	public MagacinVrsta Vrsta { get; set; }
}
