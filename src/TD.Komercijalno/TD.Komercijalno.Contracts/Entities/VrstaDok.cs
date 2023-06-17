using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("VRSTADOK")]
    public class VrstaDok
    {
        [Key]
        [Column("VRDOK")]
        public int VrDok { get; set; }
        [Column("NAZIVDOK")]
        public string NazivDok { get; set; } = "Undefined";
        [Column("POSLEDNJI")]
        public int? Poslednji { get; set; }
    }
}
