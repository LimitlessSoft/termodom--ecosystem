using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities;

[Table("PARAMETRI")]
public class Parametar
{
    [Key]
    [Column("NAZIV")]
    public string Naziv { get; set; }
    
    [Column("VREDNOST")]
    public string Vrednost { get; set; }
}