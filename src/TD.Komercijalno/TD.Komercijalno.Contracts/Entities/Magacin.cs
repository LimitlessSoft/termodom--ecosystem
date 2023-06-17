using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("MAGACIN")]
    public class Magacin
    {
        [Column("MAGACINID")]
        public int MagacinId { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; } = "Undefined";
        [Column("MTID")]
        public string MtId { get; set; } = "Undefined";
    }
}
