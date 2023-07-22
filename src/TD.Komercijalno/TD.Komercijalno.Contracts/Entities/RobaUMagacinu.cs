using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("ROBAUMAGACINU")]
    public class RobaUMagacinu
    {
        [Column("MAGACINID")]
        public short MagacinId { get; set; }
        [Column("ROBAID")]
        public int RobaId { get; set; }
        [Column("NABAVNACENA")]
        public double NabavnaCena { get; set; }
        [Column("PRODAJNACENA")]
        public double ProdajnaCena { get; set; }
        [Column("STANJE")]
        public double Stanje { get; set; }
    }
}
