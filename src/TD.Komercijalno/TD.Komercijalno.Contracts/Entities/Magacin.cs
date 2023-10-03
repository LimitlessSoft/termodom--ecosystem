using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TD.Core.Contracts;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("MAGACIN")]
    public class Magacin : IEntity
    {
        [Key]
        [Column("MAGACINID")]
        public int Id { get; set; }
        [Column("NAZIV")]
        public string Naziv { get; set; }
        [Column("MTID")]
        public string MtId { get; set; }
        [Column("VODISE")]
        public short VodiSe { get; set; }

        [NotMapped]
        public List<Stavka> Stavke { get; set; }

        [NotMapped]
        public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public int CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        [NotMapped]
        public int? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
