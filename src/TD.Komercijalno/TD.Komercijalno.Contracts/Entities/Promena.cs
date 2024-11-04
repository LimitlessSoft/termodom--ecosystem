using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities;

[Table("PROMENE")]
public class Promena
{
    [Key]
    [Column("PROMENAID")]
    public int Id { get; set; }
    [Column("VRSTANAL")]
    public short VrstaNaloga { get; set; }
    [Column("BRNALOGA")]
    public string BrojNaloga { get; set; }
    [Column("DATNAL")]
    public DateTime Datum { get; set; }
    [Column("KONTO")]
    public string Konto { get; set; }
    [Column("OPIS")]
    public string? Opis { get; set; }
    [Column("PPID")]
    public int? PPID { get; set; }
    [Column("BRDOK")]
    public int? BrDok { get; set; }
    [Column("VRDOK")]
    public int? VrDok { get; set; }
    [Column("DATDPO")]
    public DateTime? DATDPO { get; set; }
    [Column("DUGUJE")]
    public double? Duguje { get; set; }
    [Column("POTRAZUJE")]
    public double? Potrazuje { get; set; }
    [Column("DATVAL")]
    public DateTime? DatumValute { get; set; }
    [Column("VDUGUJE")]
    public double? VDuguje { get; set; }
    [Column("VPOTRAZUJE")]
    public double? VPotrazuje { get; set; }
    [Column("VALUTA")]
    public string? Valuta { get; set; }
    [Column("KURS")]
    public double Kurs { get; set; }
    [Column("MTID")]
    public string? MTID { get; set; }
    [Column("A")]
    public short? A { get; set; }
    [Column("STAVKAID")]
    public int? StavkaID { get; set; }
}
