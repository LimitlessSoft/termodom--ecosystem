using LSCore.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("VRSTADOKMAG")]
    public class VrstaDokMag : ILSCoreEntity
    {
        [Column("VRDOK")]
        public int VrDok { get; set; }
        [Column("POCINJEOD")]
        public int? PocinjeOd { get; set; }
        [Column("POSLEDNJI")]
        public int? Poslednji { get; set; }
        [Column("MAGACINID")]
        public int MagacinId { get; set; }

        [NotMapped]
        public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public int CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public int? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
