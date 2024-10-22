using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities;

[Table("ISTUPL")]
public class IstorijaUplata
{
    [Key]
    [Column("ISTUPLID")]
    public int Id { get; set; }
    [Column("VRDOK")]
    public int VrDok { get; set; }
    [Column("BRDOK")]
    public int BrDok { get; set; }
    [Column("DATUM")]
    public DateTime Datum { get; set; }
    [Column("IZNOS")]
    public double Iznos { get; set; }
    [Column("OPIS")]
    public string? Opis { get; set; }
    [Column("PPID")]
    public int PPID { get; set; }
    [Column("ZAPID")]
    public short? ZapId { get; set; }
    [Column("IO")]
    public short? IO { get; set; }
    [Column("NUID")]
    public short? NUID { get; set; }
    [Column("A")]
    public short? A { get; set; }
    [Column("KASAID")]
    public short? KasaId { get; set; }
    [Column("MTID")]
    public string? MTID { get; set; }
    [Column("VALUTA")]
    public string Valuta { get; set; }
    [Column("KURS")]
    public double Kurs { get; set; }
    [Column("PROMENAID")]
    public int? PromenaId { get; set; }
    [Column("PKID")]
    public int PkId { get; set; }
    [Column("LINKEDID")]
    public int? LinkedId { get; set; }
    [Column("PLACANJEID")]
    public int? PlacanjeId { get; set; }
    [Column("GODINA")]
    public short? Godina { get; set; }
    [Column("POC_STAID")]
    public int? PocStaId { get; set; }
}
