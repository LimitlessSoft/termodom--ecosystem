using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("VRSTADOK")]
    public class VrstaDok : IEntity
    {
        public int Id { get => VrDok; set => VrDok = value; }
        [Key]
        [Column("VRDOK")]
        public int VrDok { get; set; }
        [Column("NAZIVDOK")]
        public string NazivDok { get; set; }
        [Column("POSLEDNJI")]
        public int? Poslednji { get; set; }
        [Column("IO")]
        public short? Io { get; set; }
        [Column("IMAKARTICU")]
        public short? ImaKarticu { get; set; }

        [NotMapped]
        public List<Dokument> Dokumenti { get; set; }
    }
}
