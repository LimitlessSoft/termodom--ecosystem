using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities;

[Table("IZVOD")]
public class Izvod
{
    [Column("IZVODID")]
    public int IzvodId { get; set; }
    [Column("VRDOK")]
    public short VrDok { get; set; }
    [Column("BRDOK")]
    public int BrDok { get; set; }
    [Column("PPID")]
    public int PPID { get; set; }
    [Column("SIFPLAC")]
    public string SifraPlacanja { get; set; }
    [Column("DUGUJE")]
    public double? Duguje { get; set; }
    [Column("POTRAZUJE")]
    public double? Potrazuje { get; set; }
    [Column("RASPOREDJEN")]
    public short? Rasporedjen { get; set; }
    [Column("RDUGUJE")]
    public double? RDuguje { get; set; }
    [Column("RPOTRAZUJE")]
    public double? RPotrazuje { get; set; }
    [Column("R")]
    public short R { get; set; }
    [Column("KONTO")]
    public string? Konto { get; set; }
    [Column("VALUTA")]
    public string Valuta { get; set; }
    [Column("KURS")]
    public double? Kurs { get; set; }
    [Column("POZNABROJ")]
    public string? PozivNaBroj { get; set; }
    [Column("ZIRORACUN")]
    public string? ZiroRacun { get; set; }
    [Column("UPPID")]
    public int UPPID { get; set; }
    [Column("SVRHA")]
    public string? Svrha { get; set; }
    [Column("ED1")]
    public string? ED1 { get; set; }
    [Column("POZNABROJ_OD")]
    public string? PozivNaBrojOd { get; set; }
    [Column("VIRMANID")]
    public int? VirmanId { get; set; }
    [Column("POPDVBROJ")]
    public string PoPdvBroj { get; set; }
}
