using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TD.Komercijalno.Contracts.Entities;

[Keyless]
[Table("DOKUMENT")]
public class Dokument
{
	[Column("VRDOK")]
	public int VrDok { get; set; }

	[Column("BRDOK")]
	public int BrDok { get; set; }

	[Column("INTBROJ")]
	public string? IntBroj { get; set; }

	[Column("KODDOK")]
	public short KodDok { get; set; }

	[Column("FLAG")]
	public short? Flag { get; set; }

	[Column("DATUM")]
	public DateTime Datum { get; set; }

	[Column("LINKED")]
	public string? Linked { get; set; }

	[Column("MAGACINID")]
	public short MagacinId { get; set; }

	[Column("PPID")]
	public int? PPID { get; set; }

	[Column("FAKTDOBIZV")]
	public string? FaktDobIzv { get; set; }

	[Column("PLACEN")]
	public short Placen { get; set; } = 0;

	[Column("DATROKA")]
	public DateTime? DatRoka { get; set; }

	[Column("NUID")]
	public short? NuId { get; set; }

	[Column("NRID")]
	public short? NrId { get; set; }

	[Column("VALUTA")]
	public string Valuta { get; set; }

	[Column("KURS")]
	public double Kurs { get; set; }

	[Column("ZAPID")]
	public short ZapId { get; set; }

	[Column("UPLACENO")]
	public decimal Uplaceno { get; set; }

	[Column("TROSKOVI")]
	public decimal Troskovi { get; set; }

	[Column("DUGUJE")]
	public decimal Duguje { get; set; }

	[Column("POTRAZUJE")]
	public decimal Potrazuje { get; set; }

	[Column("POPUST")]
	public decimal Popust { get; set; }

	[Column("RAZLIKA")]
	public decimal? Razlika { get; set; }

	[Column("DODPOREZ")]
	public decimal? DodPorez { get; set; }

	[Column("POREZ")]
	public decimal? Porez { get; set; }

	[Column("PRODVREDBP")]
	public decimal ProdVredBp { get; set; }

	[Column("KUPAC")]
	public string? Kupac { get; set; }

	[Column("OPISUPL")]
	public string? OpisUpl { get; set; }

	[Column("VRDOKIN")]
	public int? VrdokIn { get; set; }

	[Column("BRDOKIN")]
	public int? BrdokIn { get; set; }

	[Column("VRDOKOUT")]
	public int? VrdokOut { get; set; }

	[Column("BRDOKOUT")]
	public int? BrdokOut { get; set; }

	[Column("MAGID")]
	public short? MagId { get; set; }

	[Column("POPUST1PROCENAT")]
	public decimal? Popust1Procenat { get; set; }

	[Column("POPUST2PROCENAT")]
	public decimal? Popust2Procenat { get; set; }

	[Column("POZNABROJ")]
	public string? PozNaBroj { get; set; }

	[Column("POPUST3PROCENAT")]
	public decimal? Popust3Procenat { get; set; }

	[Column("KONTRBROJ")]
	public string? KontrBroj { get; set; }

	[Column("MTID")]
	public string? MtId { get; set; }

	[Column("REFID")]
	public short? RefId { get; set; }

	[Column("STATUS")]
	public short? Status { get; set; }

	[Column("PPO")]
	public short? Ppo { get; set; }

	[Column("PRENETI_POREZ")]
	public decimal? PrenetiPorez { get; set; }

	[Column("AKVRDOK")]
	public short? AkVrDok { get; set; }

	[Column("AKBRDOK")]
	public short? AkBrDok { get; set; }

	[Column("ALIASIZ")]
	public short? AliasIz { get; set; }

	[Column("ALIASU")]
	public short? AliasU { get; set; }

	[Column("PREVOZROBE")]
	public short? PrevozRobe { get; set; }

	[Column("DATUM_PDV")]
	public DateTime? DatumPdv { get; set; }

	[Column("NDID")]
	public short? NdId { get; set; }

	[Column("NABVREDNOST")]
	public decimal? NabVrednost { get; set; }

	[Column("SAT_START")]
	public string? SatStart { get; set; }

	[Column("SAT_END")]
	public string? SatEnd { get; set; }

	[Column("KNJIZNAOZ")]
	public decimal? KnjiznaOz { get; set; }

	[Column("POVRATNICE")]
	public decimal? Povratnice { get; set; }

	[Column("SINHRO")]
	public short Sinhro { get; set; }

	[Column("STORNO")]
	public decimal? Storno { get; set; }

	[Column("SMENAID")]
	public short SmenaId { get; set; }

	[Column("POR_ODB")]
	public decimal? PorOdb { get; set; }

	[Column("POPDVBROJ")]
	public string? PoPdvBroj { get; set; }

	[Column("PROMET_BEZ_NAKNADE")]
	public short PrometBezNaknade { get; set; }

	[Column("GODINA")]
	public short? Godina { get; set; }

	[NotMapped]
	public VrstaDok VrstaDok { get; set; }

	[NotMapped]
	public List<Stavka> Stavke { get; set; }
}
