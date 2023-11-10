using LSCore.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.TDOffice.Contracts.Entities
{
    [Table("DOKUMENT_TAG_IZVODI")]
    public class DokumentTagIzvod : ILSCoreEntity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("BROJ_DOKUMENTA_IZVODA")]
        public int BrojDokumentaIzvoda { get; set; }
        [Column("UNOS_POCETNOSTANJE")]
        public decimal UnosPocetnoStanje { get; set; }
        [Column("UNOS_POTRAZUJE")]
        public decimal UnosPotrazuje { get; set; }
        [Column("UNOS_DUGUJE")]
        public decimal UnosDuguje { get; set; }
        [Column("KORISNIK")]
        public int Korisnik { get; set; }

        [NotMapped]
        public bool IsActive { get; set; }
        [NotMapped]
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public int? UpdatedBy { get; set; }
        [NotMapped]
        public DateTime? UpdatedAt { get; set; }
        [NotMapped]
        public int CreatedBy { get; set; }
    }
}
