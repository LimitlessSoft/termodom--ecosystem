using System.ComponentModel.DataAnnotations;

namespace TDBrain_v3.RequestBodies.Komercijalno
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TekuciRacunInsertRequestBody
    {
        [Required]
        public int PPID { get; set; }
        [Required]
        public string Racun { get; set; } = "";
        [Required]
        public int? BankaID { get; set; }
        public string Valuta { get; set; } = "DIN";
        public double Stanje { get; set; } = 0;
        public int? MagacinID { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
