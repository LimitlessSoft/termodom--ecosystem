using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
	[Table("MESTA")]
	public class Mesto
	{
		[Key]
		[Column("MESTOID")]
		public string MestoId { get; set; }

		[Column("NAZIV")]
		public string? Naziv { get; set; }

		[Column("OKRUGID")]
		public short? OkrugId { get; set; }

		[Column("UKORIST")]
		public string? UKorist { get; set; }

		[Column("NATERETZR")]
		public string? NaTeretZR { get; set; }

		[Column("HITNO")]
		public short? Hitno { get; set; }

		[Column("SIFPLAC")]
		public short? SifraPlac { get; set; }

		[Column("UPLRAC")]
		public string? UplRac { get; set; }

		[Column("UPL_MODUL")]
		public string? UplModul { get; set; }

		[Column("UPL_POZNABROJ")]
		public string? UplPozNaBroj { get; set; }

		[Column("EKSPID")]
		public short? EkspId { get; set; }
	}
}
