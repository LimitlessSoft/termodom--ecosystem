using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Komercijalno.Contracts.Enums;

namespace TD.Komercijalno.Contracts.Entities;

[Table("MAGACIN")]
public class Magacin
{
	[Key]
	[Column("MAGACINID")]
	public int Id { get; set; }

	[Column("NAZIV")]
	public string Naziv { get; set; }

	[Column("MTID")]
	public string MtId { get; set; }

	[Column("VODISE")]
	public short VodiSe { get; set; }

	[Column("VRSTA")]
	public MagacinVrsta Vrsta { get; set; }

	[NotMapped]
	public List<Stavka> Stavke { get; set; }
}
