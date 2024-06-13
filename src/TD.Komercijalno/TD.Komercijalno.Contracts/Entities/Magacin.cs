using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LSCore.Contracts.Interfaces;

namespace TD.Komercijalno.Contracts.Entities;

[Table("MAGACIN")]
public class Magacin : ILSCoreEntity
{
    [Key]
    [Column("MAGACINID")]
    public long Id { get; set; }
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
    public long CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public long? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}