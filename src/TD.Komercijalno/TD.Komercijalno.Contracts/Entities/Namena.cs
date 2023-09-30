using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("NAMENA")]
    public class Namena : IEntity
    {
        [Column("NRID")]
        public int Id { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; }
        [Column("REDOSLED")]
        public short Redosled { get; set; }
        [Column("NAPOMENA")]
        public string Napomena { get; set; }
        [Column("PPO")]
        public short PPO { get; set; }
        [Column("PDV")]
        public short PDV { get; set; }

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
