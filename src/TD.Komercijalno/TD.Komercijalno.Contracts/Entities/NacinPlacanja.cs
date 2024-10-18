using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Interfaces;

namespace TD.Komercijalno.Contracts.Entities;

[Table("NACUPL")]
public class NacinPlacanja
{
    [Column("NUID")]
    public int NUID { get; set; }
    [Column("NAZIV")]
    public string Naziv { get; set; }
}