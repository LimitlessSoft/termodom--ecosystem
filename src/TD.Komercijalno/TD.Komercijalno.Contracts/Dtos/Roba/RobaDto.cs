using TD.Komercijalno.Contracts.Dtos.Tarife;

namespace TD.Komercijalno.Contracts.Dtos.Roba
{
    public class RobaDto
    {
        public int RobaId { get; set; }
        public string KatBr { get; set; }
        public string KatBrPro { get; set; }
        public string Naziv { get; set; }
        public short? Vrsta { get; set; }
        public short? Aktivna { get; set; }
        public string GrupaId { get; set; }
        public short? Podgrupa { get; set; }
        public string? ProId { get; set; }
        public string JM { get; set; }
        public TarifaDto Tarifa { get; set; } = new TarifaDto();
    }
}
