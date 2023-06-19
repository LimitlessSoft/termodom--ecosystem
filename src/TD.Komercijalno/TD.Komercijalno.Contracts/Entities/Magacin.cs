using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("MAGACIN")]
    public class Magacin
    {
        [Column("MAGACINID")]
        public short MagacinId { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; }
        [Column("MTID")]
        public string MtId { get; set; }
        [Column("VODISE")]
        public short VodiSe { get; set; }

        [NotMapped]
        public List<Stavka> Stavke { get; set; }
    }
}
