using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.Komercijalno.Contracts.Dtos.Dokumenti
{
    public class DokumentDto
    {
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public string? IntBroj { get; set; }
        public short KodDok { get; set; }
        public short Flag { get; set; }
        public DateTime Datum { get; set; }
        public string? Linked { get; set; }
        public short MagacinId { get; set; }
        public int? PPID { get; set; }
        public string? FaktDobIzv { get; set; }
        public short Placen { get; set; } = 0;
        public DateTime? DatRoka { get; set; }
        public short? NuId { get; set; }
        public short? NrId { get; set; }
        public string Valuta { get; set; }
        public decimal Kurs { get; set; }
        public short ZapId { get; set; }
        public decimal Uplaceno { get; set; }
        public decimal Troskovi { get; set; }
        public decimal Duguje { get; set; }
        public decimal Potrazuje { get; set; }
        public decimal Popust { get; set; }
        public decimal? Razlika { get; set; }
        public decimal? DodPorez { get; set; }
        public decimal? Porez { get; set; }
        public decimal ProdVredBp { get; set; }
        public string? Kupac { get; set; }
        public string? OpisUpl { get; set; }
        public short? VrdokIn { get; set; }
        public short? VrdokOut { get; set; }
        public short? MagId { get; set; }
        public decimal? Popust1Procenat { get; set; }
        public decimal? Popust2Procenat { get; set; }
        public string? PozNaBroj { get; set; }
        public decimal? Popust3Procenat { get; set; }
        public string? KontrBroj { get; set; }
        public string? MtId { get; set; }
        public short? RefId { get; set; }
        public short? Status { get; set; }
        public short? Ppo { get; set; }
        public decimal? PrenetiPorez { get; set; }
        public short? AkVrDok { get; set; }
        public short? AkBrDok { get; set; }
        public short? AliasIz { get; set; }
        public short? AliasU { get; set; }
        public short? PrevozRobe { get; set; }
        public DateTime? DatumPdv { get; set; }
        public short? NdId { get; set; }
        public decimal? NabVrednost { get; set; }
        public string? SatStart { get; set; }
        public string? SatEnd { get; set; }
        public decimal? KnjiznaOz { get; set; }
        public decimal? Povratnice { get; set; }
        public short Sinhro { get; set; }
        public decimal? Storno { get; set; }
        public short SmenaId { get; set; }
        public decimal? PorOdb { get; set; }
        public string? PoPdvBroj { get; set; }
        public short PrometBezNaknade { get; set; }
        public short? Godina { get; set; }
        public VrstaDokDto? VrstaDok { get; set; }
        public List<StavkaDto>? Stavke { get; set; }
    }
}
