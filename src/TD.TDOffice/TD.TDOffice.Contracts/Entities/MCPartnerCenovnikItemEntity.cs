using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.TDOffice.Contracts.Entities
{
    [Table("MC_P_DOB_CENOVNIK")]
    public class MCPartnerCenovnikItemEntity : IEntity
    {
        [Column("ID")]
        public int Id { get; set; }
        [Column("KATBR")]
        public string KatBr { get; set; }
        [Column("VPCENABEZRABATA")]
        public double VpCenaBezRabata { get; set; }
        [Column("RABAT")]
        public double Rabat { get; set; }
        [Column("PPID")]
        public int PPID { get; set; }
        [Column("VAZI_OD_DANA")]
        public DateTime VaziOdDana { get; set; }

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
