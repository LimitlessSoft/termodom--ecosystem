using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Komercijalno.Contracts.Enums;

namespace TD.Komercijalno.Contracts.Entities;

[Table("VRSTADOK")]
public class VrstaDok
{
	[Key]
	[Column("VRDOK")]
	public int Id { get; set; }

	[Column("NAZIVDOK")]
	public string NazivDok { get; set; }

	[Column("POSLEDNJI")]
	public int? Poslednji { get; set; }

	[Column("IO")]
	public short? Io { get; set; }

	[Column("IMAKARTICU")]
	public short? ImaKarticu { get; set; }

	[Column("DEFINISECENU")]
	public short DefiniseCenu { get; set; }

	[Column("VRSTA")]
	public DokumentVrsta Vrsta { get; set; }

	[NotMapped]
	public List<Dokument> Dokumenti { get; set; }
}
