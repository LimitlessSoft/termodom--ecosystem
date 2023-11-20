using LSCore.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("NACIN_PLACANJA")]
    public class NacinPlacanja : ILSCoreEntity
    {
        [Column("NPID")]
        public int Id { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; }

        [NotMapped]
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public int? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public int CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
