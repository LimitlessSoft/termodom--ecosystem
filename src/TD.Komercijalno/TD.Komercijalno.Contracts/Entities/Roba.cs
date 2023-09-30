using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("ROBA")]
    public class Roba : IEntity
    {
        [Key]
        [Column("ROBAID")]
        public int Id { get; set; }
        [Column("KATBR")]
        public string KatBr { get; set; }
        [Column("KATBRPRO")]
        public string KatBrPro { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; }
        [Column("VRSTA")]
        public short? Vrsta { get; set; }
        [Column("AKTIVNA")]
        public short? Aktivna { get; set; }
        [Column("GRUPAID")]
        public string GrupaId { get; set; }
        [Column("PODGRUPA")]
        public short? Podgrupa { get; set; }
        [Column("PROID")]
        public string? ProId { get; set; }
        [Column("JM")]
        public string JM { get; set; }
        [Column("TARIFAID")]
        public string TarifaId { get; set; }

        [NotMapped]
        public Tarifa Tarifa { get; set; }

        [NotMapped]
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public long? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
