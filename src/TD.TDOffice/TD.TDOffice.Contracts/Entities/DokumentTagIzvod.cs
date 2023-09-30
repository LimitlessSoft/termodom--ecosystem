using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.TDOffice.Contracts.Entities
{
    [Table("DOKUMENT_TAG_IZVODI")]
    public class DokumentTagIzvod : IEntity
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
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public long? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int? IEntity.UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
