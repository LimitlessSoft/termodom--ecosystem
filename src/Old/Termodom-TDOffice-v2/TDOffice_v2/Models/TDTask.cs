using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Models
{
    public static class TDTask
    {
        /// <summary>
        /// Automatski generise vanredni popis
        /// </summary>
        /// <param name="magacinID">Magacin za koji se popis generise</param>
        /// <param name="vlasnikPopisa">TDOffice user ID kao vlasnik popisa</param>
        /// <exception cref="NullReferenceException"></exception>
        public static void GenerisiPopis(int magacinID, int vlasnikPopisa)
        {
            TDOffice.User korisnik = TDOffice.User.Get(vlasnikPopisa);

            if (korisnik == null)
                throw new NullReferenceException("Korisnik nije pronadjen!");

            if (korisnik.KomercijalnoUserID == null)
                throw new NullReferenceException("Korisnik nema dodeljen komercijalno ID");

            Task<List<TDOffice.DokumentPopis>> skorasnjiPopisi = Task.Run(() => { return TDOffice.DokumentPopis.List("DATUM >= '" + DateTime.Now.AddDays(15).ToString("dd.MM.yyyy") + "'"); });
            Task<List<TDOffice.StavkaPopis>> stavkeSkorasnjihPopisa = Task.Run(() =>
            {
                List<TDOffice.StavkaPopis> stavke = TDOffice.StavkaPopis.List();
                int[] brojeviSkorasnjhPopisa = skorasnjiPopisi.Result.Select(x => x.ID).ToArray();
                stavke.RemoveAll(x => !brojeviSkorasnjhPopisa.Contains(x.BrDok));
                return stavke;
            });

            Task<List<Komercijalno.Roba>> roba = Task.Run(() =>
            {
                var list = Komercijalno.Roba.List(DateTime.Now.Year);
                list.RemoveAll(x => x.Naziv.Length <= 6);
                return list;
            });
            Task<List<Komercijalno.RobaUMagacinu>> rums = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(magacinID);

            DateTime komPopisDate = DateTime.Now;

            while (komPopisDate.DayOfWeek != DayOfWeek.Sunday)
                komPopisDate = komPopisDate.AddDays(-1);

            int newPopisID = TDOffice.DokumentPopis.Insert(vlasnikPopisa, magacinID, 0, null, null, TDOffice.PopisType.Vanredni, null, null);
            int komPopis = Komercijalno.Dokument.Insert(DateTime.Now.Year, 7, "TD2 " + newPopisID.ToString(), null, "TDOffice_v2", 1, magacinID, korisnik.KomercijalnoUserID, null);
            
            TDOffice.DokumentPopis popis = TDOffice.DokumentPopis.Get(newPopisID);

            Komercijalno.Dokument komDok = Komercijalno.Dokument.Get(DateTime.Now.Year, 7, komPopis);
            komDok.Datum = komPopisDate;
            komDok.Update();

            popis.KomercijalnoPopisBrDok = komPopis;
            popis.Update();

            int ubacenoStavki = 0;
            int nTries = 0;

            while (ubacenoStavki < 5 && nTries < 10)
            {
                // Spremam string u koji cu skladistiti ciljanu popisnu grupu
                string popisnaGrupa = "?";

                // Nasumicno selektujem proizvod iz liste robe i vadim popisnu grupu iz istog
                while (popisnaGrupa.Length == 1)
                {
                    try { popisnaGrupa += roba.Result[Random.Next(roba.Result.Count - 1)].KatBrPro.Split('?')[1]; } catch { }
                }

                // Na osnovu gore definisane popisne grupe selektujem proizvode sa datom grupom
                List<Komercijalno.Roba> robaZaPopis = roba.Result.Where(x => x.KatBrPro.Contains(popisnaGrupa)).ToList();

                // Prolazim kroz robu za popis i insertujem je u dokumenta
                foreach(Komercijalno.Roba r in robaZaPopis)
                {
                    // Proveravam da li je proizvod skoro popisivan. Ako jeste onda ga zaobilazim
                    if (stavkeSkorasnjihPopisa.Result.Any(x => x.RobaID == r.ID))
                    {
                        // Uklanjam proizvod iz liste robe kako ne bi ponovo uhvatio datu robu u sledecem loopu ako dodje do istog
                        roba.Result.RemoveAll(x => x.ID == r.ID);
                        continue;
                    }

                    Komercijalno.RobaUMagacinu rum = rums.Result.FirstOrDefault(x => x.RobaID == r.ID);

                    // Dati magacin nema datu robu te je preskacem
                    if (rum == null)
                        continue;

                    // Insertujem proizvod u dokuemta
                    TDOffice.StavkaPopis.Insert(popis.ID, r.ID, rum.ProdajnaCena, 0, 0, 0);
                    Komercijalno.Stavka.Insert(DateTime.Now.Year, komDok, r, rum, 0, 0);

                    // Uklanjam proizvod iz liste robe kako ne bi ponovo uhvatio datu robu u sledecem loopu ako dodje do istog
                    roba.Result.RemoveAll(x => x.ID == r.ID);

                    ubacenoStavki ++;

                    if (ubacenoStavki > 20)
                        break;
                }

                nTries++;
            }

            TDOffice.Poruka.Insert(new TDOffice.Poruka()
            {
                Datum = DateTime.Now,
                Naslov = "Popisi Robu",
                Posiljalac = -1,
                Primalac = korisnik.ID,
                Status = TDOffice.PorukaTip.Standard,
                Tag = new TDOffice.PorukaAdditionalInfo()
                {
                    Action = TDOffice.PorukaAction.NoviTDOfficePopis,
                    AdditionalInfo = Convert.ToInt32(popis.ID)
                },
                Tekst = "Popisi i dopuni robu u popisu. Pristupi popisu klikom na dugme gore desno."
            });
        }
    }
}
