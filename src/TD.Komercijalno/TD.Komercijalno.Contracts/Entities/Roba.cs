using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LSCore.Contracts.Interfaces;
using System.Diagnostics.Contracts;

namespace TD.Komercijalno.Contracts.Entities;

[Table("ROBA")]
public class Roba
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
    [Column("NABAVNACENA")]
    public double NabavnaCena { get; set; }
    [Column("PRODAJNACENA")]
    public double ProdajnaCena { get; set; }
    [Column("DEVNABCENA")]
    public double DeviznaNabavnaCena { get; set; }
    [Column("FABRCENA")]
    public double? FabrickaCena { get; set; }
    [Column("STANJE")]
    public double Stanje { get; set; }
    [Column("NARUCENO")]
    public double Naruceno { get; set; }
    [Column("REZERVISANO")]
    public double? Rezervisano { get; set; }
    [Column("STANJEPOOTP")]
    public double StanjePoOtpremnicama { get; set; }
    [Column("TAKSA")]
    public double? Taksa { get; set; }
    [Column("MARZA")]
    public double? Marza { get; set; }
    [Column("UVOZ")]
    public short Uvoz { get; set; }
    [Column("TARBROJ")]
    public string? TarBroj { get; set; }
    [Column("AKCIZA")]
    public double? Akciza { get; set; }
    [Column("NAZIVZACARINU")]
    public string NazivZaCarinu { get; set; }
    [Column("NAZIVNAENG")]
    public string? NazivNaEngleskom { get; set; }
    [Column("GARANTID")]
    public short? GarantId { get; set; }
    [Column("ALTJM")]
    public string AltJM { get; set; }
    [Column("ALTKOL")]
    public double AltKolicina { get; set; }
    [Column("ALTNEDELJIVA")]
    public short? AltNedeljiva { get; set; }
    [Column("TRPAK")]
    public string? TransportnoPakovanje { get; set; }
    [Column("TRKOL")]
    public double? TransportnaKolicina { get; set; }
    [Column("JMSD")]
    public string? JMSD { get; set; }
    [Column("KOMENTAR")]
    public string? Komentar { get; set; }
    [Column("XOD")]
    public double? XOD { get; set; }
    [Column("XDO")]
    public double? XDO { get; set; }
    [Column("YOD")]
    public double? YOD { get; set; }
    [Column("YDO")]
    public double? YDO { get; set; }
    [Column("ZOD")]
    public double? ZOD { get; set; }
    [Column("ZDO")]
    public double? ZDO { get; set; }
    [Column("IMAROKTRAJANJA")]
    public short? ImaRokTrajanja { get; set; }
    [Column("NACENOVNIKU")]
    public short? NaCenovniku { get; set; }
    [Column("ZAPID")]
    public short? ZapId { get; set; }
    [Column("NORMA")]
    public double? Norma { get; set; }
    [Column("KALO")]
    public double? Kalo { get; set; }
    [Column("TEZINA")]
    public double? Tezina { get; set; }
    [Column("PIN")]
    public string? Pin { get; set; }
    [Column("KRITZAL")]
    public double KriticnaZaliha { get; set; }
    [Column("OPTZAL")]
    public double OptimalnaZaliha { get; set; }
    [Column("KATEGORIJA")]
    public Int64? Kategorija { get; set; }
    [Column("IMASBROJ")]
    public short? ImaSBroj { get; set; }
    [Column("STANJEPOSER")]
    public double StanjePoSer { get; set; }
    [Column("ZAPREMINA")]
    public double? Zapremnina { get; set; }
    [Column("SLIKA")]
    public string? Slika { get; set; }
    [Column("PPID")]
    public int PPID { get; set; }
    [Column("TRDECPAK")]
    public short? TrDecPak { get; set; }
    [Column("PRODCENABP")]
    public double ProdCenaBP { get; set; }
    [Column("JMR")]
    public string JMR { get; set; }
    [Column("STANJEPOREKLAM")]
    public double? StanjePoReklamacijama { get; set; }
    [Column("STANJEPOREVERSU")]
    public double? StanjePoReversu { get; set; }
    [Column("ADR")]
    public double? Adr { get; set; }
    [Column("STANJE_MOJE_EKSP")]
    public double? StanjeMojeEkspoziture { get; set; }
    [Column("VPCID")]
    public short? VPCID { get; set; }
    [Column("PROCPC")]
    public double? PROCPC { get; set; }
    [Column("DATUM_ISPORUKE")]
    public DateTime? DatumIsporuke { get; set; }
    [Column("REZERVISANO_MOJE_EKSP")]
    public double? RezervisanoMojeEkspoziture { get; set; }
    [Column("STANJEPOOTP_MOJE_EKSP")]
    public double? StanjePoOtpremnicamaMojeEkspoziture { get; set; }
    [Column("STANJEPOSER_MOJE_EKSP")]
    public double? StanjePoSerMojeEkspozitorue { get; set; }
    [Column("NAZIVZASTAMPU")]
    public string? NazivZaStampu { get; set; }
    [Column("ALTPIN")]
    public string? AltPin { get; set; }
    [Column("TRPIN")]
    public string? TRPin { get; set; }
    [Column("DRZAVAID")]
    public short? DrzavaId { get; set; }
    [Column("LINKED_ROBAID")]
    public int? LinkedRobaId { get; set; }
    [Column("OBLIK")]
    public short? Oblik { get; set; }
    [Column("REKLAM_PROC")]
    public double? ReklamProc { get; set; }
    [Column("JM_POVRSINA")]
    public double? JmPovrsina { get; set; }
    [Column("JM_ZAPREMINA")]
    public double? JmZapremina { get; set; }
    [Column("X3")]
    public double? X3 { get; set; }
    [Column("Y3")]
    public double? Y3 { get; set; }
    [Column("Z3")]
    public double? Z3 { get; set; }
    [Column("NAS_BARKOD")]
    public short? NasBarKod { get; set; }
    [Column("REFVAL")]
    public string? RefVal { get; set; }
    [Column("KGID")]
    public int? KgId { get; set; }
    [Column("DOB_PPID")]
    public int? DobPPID { get; set; }
    [Column("BOJA")]
    public int? Boja { get; set; }
    [Column("TD")]
    public short? TD { get; set; }
    [Column("KAT")]
    public string? KAT { get; set; }
    [Column("GKID")]
    public string? GKID { get; set; }
    [Column("WEB_UPDATE")]
    public short? WebUpdate { get; set; }
    [Column("WEB_KOL_KOR")]
    public double? WebKolKor { get; set; }
    [Column("TD_GRUPAID")]
    public short? TDGrupaId { get; set; }
    [Column("USL_SKLAD")]
    public string? UslSklad { get; set; }
    [Column("NETO_TEZINA")]
    public double? NetoTezina { get; set; }
    [Column("PDV_NA_RUC")]
    public short? PdvNaRuc { get; set; }

    [NotMapped]
    public Tarifa Tarifa { get; set; }
}