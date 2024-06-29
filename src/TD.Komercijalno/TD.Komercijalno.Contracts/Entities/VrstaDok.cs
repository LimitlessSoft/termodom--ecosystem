using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LSCore.Contracts.Interfaces;

namespace TD.Komercijalno.Contracts.Entities;

[Table("VRSTADOK")]
public class VrstaDok : ILSCoreEntity
{
    [Key]
    [Column("VRDOK")]
    public int Id { get; set; }
    [Column("NAZIVDOK")]
    public string NazivDok { get; set; }
    [Column("POSLEDNJI")]
    public int? Poslednji { get; set; }
    [Column("IO")]
    public short? Io { get; set; }
    [Column("IMAKARTICU")]
    public short? ImaKarticu { get; set; }
    [Column("DEFINISECENU")]
    public short DefiniseCenu { get; set; }


    [NotMapped]
    public List<Dokument> Dokumenti { get; set; }

    [NotMapped]
    public bool IsActive { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public long? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public DateTime? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    [NotMapped]
    public long CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}