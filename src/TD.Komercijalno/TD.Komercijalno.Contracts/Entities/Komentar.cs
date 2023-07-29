using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("KOMENTARI")]
    public class Komentar
    {
        [Column("VRDOK")]
        public int VrDok { get; set; }
        [Column("BRDOK")]
        public int BrDok { get; set; }
        [Column("KOMENTAR")]
        public string? JavniKomentar { get; set; }
        [Column("INTKOMENTAR")]
        public string? InterniKomentar { get; set; }
        [Column("PRIVKOMENTAR")]
        public string? PrivatniKomentar { get; set; }
    }
}
