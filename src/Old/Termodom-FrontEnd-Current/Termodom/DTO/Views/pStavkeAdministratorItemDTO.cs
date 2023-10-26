using Termodom.Models;

namespace Termodom.DTO.Views
{
    public class pStavkeAdministratorItemDTO
    {
        public Porudzbina Porudzbina { get; set; }
        public Proizvod Proizvod { get; set; }
        public Porudzbina.Stavka Stavka { get; set; }
        public Cenovnik IronCenovnik { get; set; }
        public Cenovnik SilverCenovnik { get; set; }
        public Cenovnik GoldCenovnik { get; set; }
        public Cenovnik PlatinumCenovnik { get; set; }

        public pStavkeAdministratorItemDTO()
        {

        }
        public pStavkeAdministratorItemDTO(Porudzbina porudzbina, Proizvod proizvod, Porudzbina.Stavka stavka, Cenovnik ironCenovnik,
            Cenovnik silverCenovnik, Cenovnik goldCenovnik, Cenovnik platinumCenovnik)
        {
            this.Porudzbina = porudzbina;
            this.Proizvod = proizvod;
            this.Stavka = stavka;
            this.IronCenovnik = ironCenovnik;
            this.SilverCenovnik = silverCenovnik;
            this.GoldCenovnik = goldCenovnik;
            this.PlatinumCenovnik = platinumCenovnik;
        }
    }
}
