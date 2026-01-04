using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities;

[Table("INTERTRANSPORT")]
public class InterniTransport
{
	[Column("ITID")]
	public int Id { get; set; }

	[Column("IZVRDOK")]
	public short IzVrDok { get; set; }

	[Column("IZBRDOK")]
	public int IzBrDok { get; set; }

	[Column("UVRDOK")]
	public short UVrDok { get; set; }

	[Column("UBRDOK")]
	public int UBrDok { get; set; }
}
