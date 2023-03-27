using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.Models
{
    public static class JednokratnaKupovina
    {
        private static int _nSegment = 20;
        private static double _vrednostSegmenta = 9000;
        public static double VrednostSegmenta()
        {
            return _vrednostSegmenta;
        }
        public static int StepenRabata(double ukupnaVrednost)
        {
            double segmnet = ukupnaVrednost / _vrednostSegmenta;
            if (segmnet > _nSegment)
                segmnet = _nSegment;

            if (segmnet % 1 != 0)
                segmnet = segmnet - (segmnet % 1);
            

            return (int)segmnet;

        }
        public static Cenovnik GetCenovnik(Korpa korpa)
        {
            Cenovnik c = new Cenovnik();

            double zbirKorpe = korpa.Zbir();
            int nivoKorpe = Math.Min(_nSegment, (int)(zbirKorpe / _vrednostSegmenta));

            List<Proizvod> sviProizvodi = Proizvod.List();

            foreach(Proizvod p in sviProizvodi)
            {
                double prodajnaCenaZaOvuKorpu = -1;

                double r = p.ProdajnaCena - p.NabavnaCena;
                if (p.ProdajnaCena * 0.15 < r) // Ovo ogranicava na max 15% rabata
                    r = p.ProdajnaCena * 0.15;
                double razlikaZaIgru = r * (1 - Cenovnik.OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH);
                prodajnaCenaZaOvuKorpu = p.ProdajnaCena - (razlikaZaIgru / 15 * nivoKorpe);
                c.Add(new Cenovnik.Artikal() { ID = p.RobaID, Cena = new Cena() { VPCena = prodajnaCenaZaOvuKorpu } });
            }

            return c;
        }

        public static Cenovnik CenovnikPoPorudzbini(Porudzbina porudzbina)
        {
            Cenovnik c = new Cenovnik();
            List<Proizvod> list = Proizvod.List();
            double zbirKorpe = 0;
            foreach (Porudzbina.Stavka s in porudzbina.Stavke)
            {
                zbirKorpe += list.Where(t => t.RobaID == s.RobaID).FirstOrDefault().ProdajnaCena; // TODO: .FirstOrDefault(Expression)

            }
            int nivoKorpe = Math.Min(_nSegment, (int)(zbirKorpe / _vrednostSegmenta));

            foreach (Proizvod p in list)
            {
                double prodajnaCenaZaOvuKorpu = -1;
                double r = p.ProdajnaCena - p.NabavnaCena;
                if (p.ProdajnaCena * 0.15 > r) // Ovo ogranicava na max 15% rabata
                    r = p.ProdajnaCena * 0.15;
                double razlikaZaIgru = r * (1 - Cenovnik.OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH);
                prodajnaCenaZaOvuKorpu = p.ProdajnaCena - (razlikaZaIgru / 15 * nivoKorpe);
                c.Add(new Cenovnik.Artikal() { ID = p.RobaID, Cena = new Cena() { VPCena = prodajnaCenaZaOvuKorpu } });
            }

            return c;
        }
    }
}
