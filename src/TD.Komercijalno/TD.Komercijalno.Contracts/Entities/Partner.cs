using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Entities
{
    [Table("PARTNER")]
    public class Partner
    {
        [Key]
        [Column("PPID")]
        public int Ppid { get; set; }

        [Column("NAZIV")]
        public string Naziv { get; set; }

        [Column("ADRESA")]
        public string Adresa { get; set; }

        [Column("POSTA")]
        public string Posta { get; set; }

        [Column("MESTO")]
        public string Mesto { get; set; }

        [Column("TELEFON")]
        public string Telefon { get; set; }

        [Column("FAX")]
        public string Fax { get; set; }

        [Column("EMAIL")]
        public string Email { get; set; }

        [Column("KONTAKT")]
        public string Kontakt { get; set; }

        [Column("POPUST")]
        public decimal? Popust { get; set; }

        [Column("VAZIDANA")]
        public short? VaziDana { get; set; }

        [Column("NEPLFAKT")]
        public short NeplFakt { get; set; }

        [Column("DUGUJE")]
        public decimal Duguje { get; set; }

        [Column("POTRAZUJE")]
        public decimal Potrazuje { get; set; }

        [Column("VPCID")]
        public short? VpcId { get; set; }

        [Column("PROCPC")]
        public decimal? ProcPc { get; set; }

        [Column("KATEGORIJA")]
        public long? Kategorija { get; set; }

        [Column("MBROJ")]
        public string Mbroj { get; set; }

        [Column("SDEL")]
        public string Sdel { get; set; }

        [Column("NPPID")]
        public int? Nppid { get; set; }

        [Column("MESTOID")]
        public string MestoId { get; set; }

        [Column("IMAUGOVOR")]
        public short? ImaUgovor { get; set; }

        [Column("IMAIZJAVE")]
        public short? ImaIzjave { get; set; }

        [Column("CENOVNIKID")]
        public short? CenovnikId { get; set; }

        [Column("RBROJ")]
        public string Rbroj { get; set; }

        [Column("PRISTUPNINIVO")]
        public int? PristupniNivo { get; set; }

        [Column("NEFAKOTP")]
        public decimal NefakOtp { get; set; }

        [Column("ZAPID")]
        public short? ZapId { get; set; }

        [Column("VRSTACENOVNIKA")]
        public short? VrstaCenovnika { get; set; }

        [Column("PRAVNOLICE")]
        public short? PravnoLice { get; set; }

        [Column("DELATNOSTID")]
        public string DelatnostId { get; set; }

        [Column("REFID")]
        public short? RefId { get; set; }

        [Column("VALUTA")]
        public string Valuta { get; set; }

        [Column("DOZVOLJENIMINUS")]
        public decimal? DozvoljeniMinus { get; set; }

        [Column("PIB")]
        public string Pib { get; set; }

        [Column("UVPCID")]
        public short? UvpcId { get; set; }

        [Column("UPROCPC")]
        public decimal? UprocPc { get; set; }

        [Column("PDVO")]
        public short? Pdvo { get; set; }

        [Column("DRZAVLJANSTVOID")]
        public short? DrzavljanstvoId { get; set; }

        [Column("ZANIMANJEID")]
        public short? ZanimanjeId { get; set; }

        [Column("PREVOZROBE")]
        public short? PrevozRobe { get; set; }

        [Column("WEB_KORISNIK")]
        public string WebKorisnik { get; set; }

        [Column("WEB_STATUS")]
        public short? WebStatus { get; set; }

        [Column("DRZAVAID")]
        public short? DrzavaId { get; set; }

        [Column("WEB_MAGACINID")]
        public short? WebMagacinId { get; set; }

        [Column("DUG_KUPAC")]
        public decimal? DugKupac { get; set; }

        [Column("POT_KUPAC")]
        public decimal? PotKupac { get; set; }

        [Column("DUG_DOBAV")]
        public decimal? DugDobav { get; set; }

        [Column("POT_DOBAV")]
        public decimal? PotDobav { get; set; }

        [Column("VPC_RABAT")]
        public short? VpcRabat { get; set; }

        [Column("WEB_DOC_CONV")]
        public short? WebDocConv { get; set; }

        [Column("AKTIVAN")]
        public short? Aktivan { get; set; }

        [Column("NAZIVZASTAMPU")]
        public string NazivZaStampu { get; set; }

        [Column("MOBILNI")]
        public string Mobilni { get; set; }

        [Column("WEB_PRIVILEGIJE")]
        public long? WebPrivilegije { get; set; }

        [Column("GPPID")]
        public int? Gppid { get; set; }

        [Column("CENE_OD_GRUPE")]
        public short? CeneOdGrupe { get; set; }

        [Column("POL")]
        public short? Pol { get; set; }

        [Column("WEB_DANA_NEAKTIVAN")]
        public short? WebDanaNeaktivan { get; set; }

        [Column("WEB_NEAKTIVAN_STATUS")]
        public short? WebNeaktivanStatus { get; set; }

        [Column("WEB")]
        public short? Web { get; set; }

        [Column("WEB_USERID")]
        public int? WebUserId { get; set; }

        [Column("WEB_SHOP_USER")]
        public string WebShopUser { get; set; }

        [Column("WEB_SHOP_PASS")]
        public string WebShopPass { get; set; }

        [Column("WEB_SHOP_CONV")]
        public short? WebShopConv { get; set; }

        [Column("OPSTINAID")]
        public short? OpstinaId { get; set; }

        [Column("REFERID")]
        public int? ReferId { get; set; }

        [Column("OD_DATUM_WWEB_NEAKTIV")]
        public DateTime? OdDatumWwebNeaktiv { get; set; }

        [Column("KONTO_TROSAK")]
        public string KontoTrosak { get; set; }

        [Column("KONTO_IZVOD")]
        public string KontoIzvod { get; set; }

        [Column("WEB_STAT")]
        public short? WebStat { get; set; }

        [Column("DOZVOLJENIMINUS_ROK")]
        public decimal? DozvoljeniMinusRok { get; set; }

        [Column("LINIJAID")]
        public int? LinijaId { get; set; }

        [Column("PROC_TRO")]
        public decimal? ProcTro { get; set; }

        [Column("VRSTA_UPITA")]
        public short? VrstaUpita { get; set; }

        [Column("DEVKL")]
        public short DevKl { get; set; }

        [Column("PGRUID")]
        public int? Pgruid { get; set; }

        [Column("EKSPID")]
        public short? Ekspid { get; set; }

        [Column("SAMO_AVANS")]
        public short? SamoAvans { get; set; }

        [Column("ZID")]
        public short? Zid { get; set; }

        [Column("SEFID")]
        public short? Sefid { get; set; }

        [Column("TKID")]
        public short? Tkid { get; set; }

        [Column("PROMID")]
        public short? Promid { get; set; }

        [Column("NADIMAK")]
        public string Nadimak { get; set; }

        [Column("GLN")]
        public string Gln { get; set; }

        [Column("NAS_GLN")]
        public string NasGln { get; set; }

        [Column("PRIMA_EF")]
        public short? PrimaEf { get; set; }

        [Column("EF_13")]
        public short? Ef13 { get; set; }

        [Column("EF_40")]
        public short? Ef40 { get; set; }

        [Column("EF_41")]
        public short? Ef41 { get; set; }
    }
}
