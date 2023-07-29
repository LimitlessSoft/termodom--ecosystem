using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Dtos.Magacini
{
    public class MagacinDto
    {
        public int MagacinId { get; set; }
        public string Naziv { get; set; }
        public string MtId { get; set; }
        public short VodiSe { get; set; }
    }
}
