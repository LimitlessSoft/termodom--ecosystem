using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.TDOffice.Contracts.Entities
{
    [Table("MC_P_CEN_ROBAID")]
    public class MCPartnerCenovnikKatBrRobaIdEntity : IEntity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        [Column("CENOVNIK_KAT_BR")]
        public string KatBrProizvodjaca { get; set; }
        [Column("ROBAID")]
        public int RobaId { get; set; }
        [Column("PROIZVODJAC")]
        public string Proizvodjac { get; set; }

        [NotMapped]
        public bool IsActive { get; set; }
        [NotMapped]
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public int CreatedBy { get; set; }
        [NotMapped]
        public int? UpdatedBy { get; set; }
        [NotMapped]
        public DateTime? UpdatedAt { get; set; }
    }
}
