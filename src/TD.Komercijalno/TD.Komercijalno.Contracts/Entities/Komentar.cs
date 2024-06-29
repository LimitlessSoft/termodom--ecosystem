using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Contracts.Interfaces;

namespace TD.Komercijalno.Contracts.Entities;

[Table("KOMENTARI")]
public class Komentar : ILSCoreEntity
{
    [Column("VRDOK")]
    public int VrDok { get; set; }
    [Column("BRDOK")]
    public int BrDok { get; set; }
    [Column("KOMENTAR")]
    public string? JavniKomentar { get; set; }
    [Column("INTKOMENTAR")]
    public string? InterniKomentar { get; set; }
    [Column("PRIVKOMENTAR")]
    public string? PrivatniKomentar { get; set; }

    [NotMapped]
    public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public long CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public long? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}