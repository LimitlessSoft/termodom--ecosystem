using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Interfaces;

namespace TD.Komercijalno.Contracts.Entities;

[Table("NAMENA")]
public class Namena
{
    [Column("NRID")]
    public long Id { get; set; }
    [Column("NAZIV")]
    public string Naziv { get; set; }
    [Column("REDOSLED")]
    public short Redosled { get; set; }
    [Column("NAPOMENA")]
    public string Napomena { get; set; }
    [Column("PPO")]
    public short PPO { get; set; }
    [Column("PDV")]
    public short PDV { get; set; }
}