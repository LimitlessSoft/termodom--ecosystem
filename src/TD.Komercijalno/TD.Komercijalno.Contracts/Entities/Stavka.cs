using LSCore.Contracts.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("STAVKA")]
    public class Stavka : ILSCoreEntity
    {
        [Key]
        [Column("STAVKAID")]
        public int Id { get; set; }
        [Column("VRDOK")]
        public int VrDok { get; set; }
        [Column("BRDOK")]
        public int BrDok { get; set; }
        [Column("MAGACINID")]
        public int MagacinId { get; set; }
        [Column("ROBAID")]
        public int RobaId { get; set; }
        [Column("VRSTA")]
        public short? Vrsta { get; set; }
        [Column("NAZIV")]
        public string? Naziv { get; set; }
        [Column("NABCENSAPOR")]
        public double? NabCenSaPor { get; set; }
        [Column("FAKTURNACENA")]
        public double? FakturnaCena { get; set; }
        [Column("NABAVNACENA")]
        public double NabavnaCena { get; set; }
        [Column("PRODCENABP")]
        public double? ProdCenaBp { get; set; }
        [Column("KOREKCIJA")]
        public double? Korekcija { get; set; }
        [Column("PRODAJNACENA")]
        public double ProdajnaCena { get; set; }
        [Column("KOLICINA")]
        public double Kolicina { get; set; }
        [Column("TARIFAID")]
        public string? TarifaId { get; set; }
        [Column("IMAPOREZ")]
        public short? ImaPorez { get; set; }
        [Column("POREZ")]
        public double? Porez { get; set; }
        [Column("POREZ_ULAZ")]
        public double? PorezUlaz { get; set; }
        [Column("POREZ_IZ")]
        public double? PorezIzlaz { get; set; }
        [Column("MTID")]
        public string? MtId { get; set; }
        [Column("DEVIZNACENA")]
        public double DeviznaCena { get; set; }
        [Column("DEVPRODCENA")]
        public double? DevProdCena { get; set; }
        [Column("NIVKOL")]
        public double NivelisanaKolicina { get; set; }
        [Column("RABAT")]
        public double Rabat { get; set; }
        [Column("MARZA")]
        public double Marza { get; set; }
        [Column("PROSNAB")]
        public double ProsNab { get; set; }
        [Column("PRECENA")]
        public double PreCena { get; set; }
        [Column("PRENAB")]
        public double PreNab { get; set; }
        [Column("PROSPROD")]
        public double ProsProd { get; set; }
        [Column("NABCENABT")]
        public double? NabCenaBt { get; set; }
        [Column("TROSKOVI")]
        public double? Troskovi { get; set; }

        [NotMapped]
        public Dokument Dokument { get; set; }
        [NotMapped]
        public Magacin Magacin { get; set; }

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