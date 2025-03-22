using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities;

[Table("VRSTADOKMAG")]
public class VrstaDokMag
{
	[Column("VRDOK")]
	public int VrDok { get; set; }

	[Column("POCINJEOD")]
	public int? PocinjeOd { get; set; }

	[Column("POSLEDNJI")]
	public int? Poslednji { get; set; }

	[Column("MAGACINID")]
	public int MagacinId { get; set; }
}
