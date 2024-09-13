using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("PPKATEGORIJE")]
    public class PPKategorija
    {
        [Key]
        [Column("KATID")]
        public int KatId { get; set; }

        [Column("KATNAZIV")]
        public string KatNaziv { get; set; }
    }
}
