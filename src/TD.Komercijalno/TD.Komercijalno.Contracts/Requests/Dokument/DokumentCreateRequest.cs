using LSCore.Contracts.Requests;

namespace TD.Komercijalno.Contracts.Requests.Dokument
{
    public class DokumentCreateRequest : LSCoreSaveRequest
    {
        public int VrDok { get; set; }
        public string? IntBroj { get; set; }
        public short KodDok { get; set; }
        public short Flag { get; set; }
        public DateTime Datum { get; set; } = DateTime.Now;
        public string? Linked { get; set; }
        public short MagacinId { get; set; }
        public int? PPID { get; set; }
        public string? FaktDobIzv { get; set; }
        public short Placen { get; set; } = 0;
        public DateTime? DatRoka { get; set; } = DateTime.Now;
        public short? NuId { get; set; }
        public short? NrId { get; set; }
        public string Valuta { get; set; } = "DIN";
        public decimal Kurs { get; set; } = 1;
        public short ZapId { get; set; }
        public decimal Uplaceno { get; set; } = 0;
        public decimal Troskovi { get; set; } = 0;
        public decimal Duguje { get; set; } = 0;
        public decimal Potrazuje { get; set; } = 0;
        public decimal Popust { get; set; } = 0;
        public decimal? Razlika { get; set; }
        public decimal? DodPorez { get; set; }
        public decimal? Porez { get; set; }
        public decimal ProdVredBp { get; set; } = 0;
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
        public short? Status { get; set; } = 0;
        public short? Ppo { get; set; } = 0;
        public decimal? PrenetiPorez { get; set; } = 0;
        public short? AkVrDok { get; set; } = 0;
        public short? AkBrDok { get; set; } = 0;
        public short? AliasIz { get; set; }
        public short? AliasU { get; set; }
        public short? PrevozRobe { get; set; } = 0;
        public DateTime? DatumPdv { get; set; } = DateTime.Now;
        public short? NdId { get; set; } = 0;
        public decimal? NabVrednost { get; set; } = 0;
        public string? SatStart { get; set; }
        public string? SatEnd { get; set; }
        public decimal? KnjiznaOz { get; set; } = 0;
        public decimal? Povratnice { get; set; } = 0;
        public short Sinhro { get; set; }
        public decimal? Storno { get; set; } = 0;
        public short SmenaId { get; set; } = 0;
        public decimal? PorOdb { get; set; } = 0;
        public string? PoPdvBroj { get; set; }
        public short PrometBezNaknade { get; set; } = 0;
        public short? Godina { get; set; }
    }
}
