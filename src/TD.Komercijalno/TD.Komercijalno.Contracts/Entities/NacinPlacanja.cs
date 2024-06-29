using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Interfaces;

namespace TD.Komercijalno.Contracts.Entities;

[Table("NACIN_PLACANJA")]
public class NacinPlacanja
{
    [Column("NPID")]
    public int Id { get; set; }
    [Column("NAZIV")]
    public string Naziv { get; set; }
}