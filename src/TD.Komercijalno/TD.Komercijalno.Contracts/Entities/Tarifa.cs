using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("TARIFE")]
    public class Tarifa
    {
        [Key]
        [Column("TARIFAID")]
        public string TafiraId { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; }
        [Column("STOPA")]
        public double Stopa { get; set; }
        [Column("VRSTA")]
        public short Vrsta { get; set; }

        [NotMapped]
        public List<Roba> Roba { get; set; }
    }
}
