using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Termodom.Models;


namespace Termodom.Models
{
    public partial class Korpa
    {
        public string Identifikator { get; set; }

        private List<Stavka> _stavke = new List<Stavka>();

        public Korpa.Stavka this[int id]
        {
            get
            {
                Stavka st = _stavke.Where(t => t.RobaID == id).FirstOrDefault();
                if (st == null)
                    return null;

                return st;
            }
        }

        public bool DodajStavku(int id, double kolicina)
        {
            Roba r = Roba.Get(id);

            if (r == null)
                return false;

            if (_stavke.Where(t => t.RobaID == r.ID).FirstOrDefault() == null) // TODO: _stavke.FirstOrDefault()
                _stavke.Add(new Stavka()
                {
                    Kolicina = kolicina,
                    RobaID = id
                });
            else _stavke.Where(t => t.RobaID == r.ID).ToList().ForEach(t => t.Kolicina += kolicina);

            return true;
        }
        /// <summary>
        /// Uklanja stavku iz korpe sa zadatim ID-om
        /// </summary>
        /// <param name="stavkaID"></param>
        /// <returns></returns>
        public bool UkloniStavku(int stavkaID)
        {
            Stavka stavka = _stavke.Where(t => t.RobaID == stavkaID).FirstOrDefault(); // TODO: _stavke.FirstOrDefault()
            if(stavka == null)
                return false;
            else
                _stavke.Remove(stavka);
            return true;
        }
        public List<Stavka> ListaStavki()
        {
            return _stavke;
        }
        public string ZakljucajProfiPorudzbinu(Korisnik k, string komentar, int magacin, int nacinUplate, double usteda, string adresa)
        {
            Task<List<Proizvod>> proizvodi = Proizvod.ListAsync();
            Porudzbina p = new Porudzbina()
            {
                UserID = k.ID,
                MagacinID = magacin,
                PPID = k.PPID == 0 ? null : k.PPID,
                NacinUplate = (PorudzbinaNacinUplate)nacinUplate,
                ImeIPrezime = k.Nadimak,
                Napomena = komentar,
                KontaktMobilni = k.Mobilni,
                UstedaKorisnik = usteda,
                AdresaIsporuke = adresa,
                Datum = DateTime.UtcNow
            }; // TODO: Remove

            string hash = Porudzbina.Insert(p); // TODO: Inner

            Porudzbina p1 = Porudzbina.List().Where(t => t.Hash == hash).FirstOrDefault(); // TOOD: Get

            Korisnik.Cenovnik korisnikCenovnik = k.GetCenovnik();

            foreach (Korpa.Stavka stavka in this.ListaStavki())
            {
                Korisnik.Cenovnik.Artikal artikal = korisnikCenovnik[stavka.RobaID];
                double rabat = ((proizvodi.Result.Where(t => t.RobaID == stavka.RobaID).FirstOrDefault().ProdajnaCena / artikal.Cena.VPCena ) - 1) * (100);
                Porudzbina.Stavka.Insert(p1.ID, stavka.RobaID, stavka.Kolicina, artikal.Cena.VPCena, rabat);
            }
            return hash;
        }
        public string ZakljucajJednokratnuKupovinu(string komentar, int magacin, int nacinUplate, string imePrezime, string mobilni, string adresa, double usteda)
        {
            Task<List<Proizvod>> proizvodi = Proizvod.ListAsync();
            Porudzbina p = new Porudzbina() {
                MagacinID = magacin,
                NacinUplate = (PorudzbinaNacinUplate)nacinUplate,
                ImeIPrezime = imePrezime,
                KontaktMobilni = mobilni,
                AdresaIsporuke = adresa,
                UstedaKorisnik = usteda,
                Napomena = komentar,
                Datum = DateTime.UtcNow,
                UserID = -7
            }; // TODO: Remove
            string hash = Porudzbina.Insert(p); // TODO: Inner

            Cenovnik korisnikCenovnik = JednokratnaKupovina.GetCenovnik(this);

            #region TODO
            // TODO: Porudzbina.Get
            List<Porudzbina> list = Porudzbina.List();
            Porudzbina p1 = list.Where(t=> t.Hash != null && t.Hash == hash).FirstOrDefault(); // TODO: list.Fi
            #endregion

            foreach (Korpa.Stavka stavka in this.ListaStavki())
            {
                Cenovnik.Artikal artikal = korisnikCenovnik[stavka.RobaID];

                double Rabat = ((artikal.Cena.VPCena / proizvodi.Result.FirstOrDefault(t => t.RobaID == stavka.RobaID).ProdajnaCena) - 1) * (-100);
                Porudzbina.Stavka.Insert(p1.ID, stavka.RobaID, stavka.Kolicina, artikal.Cena.VPCena, Rabat);
            }

            return hash;
        }
        public void Insert()
        {
            // TODO: ????
        }
        public double Zbir()
        {
            List<Proizvod> proizvodi = Proizvod.List();

            return _stavke.Sum(x => x.Kolicina * proizvodi.Where(y => y.RobaID == x.RobaID).FirstOrDefault().ProdajnaCena); // TODO: FirstOrDefault?????
        }
        public bool ValidacijaJednokratne(string imePrezime, string mobilni)
        {
            if (string.IsNullOrWhiteSpace(imePrezime))
                return false;
            if (imePrezime.Length < 5)
                return false;
            if (string.IsNullOrWhiteSpace(mobilni))
                return false;
            if (!mobilni.Contains('0'))
                return false;
            if (mobilni.Length < 5)
                return false;
            if (!mobilni.All(Char.IsDigit))
                return false;

            return true;
        }
    }
}
