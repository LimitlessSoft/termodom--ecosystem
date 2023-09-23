using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.TDOffice.Contracts.Entities
{
    [Table("DOKUMENT_TAG_IZVODI")]
    public class DokumentTagIzvod : IEntityBase
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
    }
}
