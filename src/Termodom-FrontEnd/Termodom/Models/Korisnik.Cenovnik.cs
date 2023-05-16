using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Termodom.Models;

namespace Termodom.Models
{
    public partial class Korisnik
    {
        public class Cenovnik : Models.Cenovnik
        {
            public static Task<Cenovnik> PlatinumCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(3);
            public static Task<Cenovnik> GoldCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(2);
            public static Task<Cenovnik> SilverCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(1);
            public static Task<Cenovnik> IronCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(0);
            public static DateTime CenovniciBufferUpdated = DateTime.Now;

            static Cenovnik()
            {
                Task.Run(() =>
                {
                    while(Program.IsRunning)
                    {
                        PlatinumCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(3);
                        GoldCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(2);
                        SilverCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(1);
                        IronCenovnik = Korisnik.Cenovnik.GetZaNivoAsync(0);

                        Task.WaitAll(PlatinumCenovnik, GoldCenovnik, SilverCenovnik, IronCenovnik);

                        CenovniciBufferUpdated = DateTime.Now;
                        Thread.Sleep(60000);
                    }
                });
            }

            /// <summary>
            /// Vraca cenovnik za izabranog korisnika
            /// </summary>
            /// <param name="korisnikID"></param>
            /// <returns></returns>
            public static Cenovnik Get(int korisnikID)
            {
                Cenovnik c = new Cenovnik();
                c.KorisnikID = korisnikID;
                List<Proizvod> sviProizvodi = Proizvod.List();

                // CenovnaGrupaID / nivo za korisnika
                List<Tuple<int, int>> uslovi = new List<Tuple<int, int>>();
                
                using(MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                {
                    con.Open();
                    using(MySqlCommand cmd = new MySqlCommand("SELECT NIVO, CENOVNIK_GRUPAID FROM USER_CENOVNIK WHERE USERID = @UID", con))
                    {
                        cmd.Parameters.AddWithValue("@UID", korisnikID);

                        using(MySqlDataReader dr = cmd.ExecuteReader())
                            while(dr.Read())
                                uslovi.Add(new Tuple<int, int>(Convert.ToInt32(dr["CENOVNIK_GRUPAID"]), Convert.ToInt32(dr["NIVO"])));
                    }

                }

                foreach(Proizvod proizvod in sviProizvodi)
                {
                    double minCena = proizvod.NabavnaCena;
                    double maxCena = proizvod.ProdajnaCena;
                    double r = maxCena - minCena;
                    double razlika = r * (1 - OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH);

                    Tuple<int, int> korisnikovCenovniUslovZaOvajProizvod = null;
                    try
                    {
                        korisnikovCenovniUslovZaOvajProizvod = uslovi.Where(x => x.Item1 == proizvod.CenovnaGrupaID).FirstOrDefault();
                    }
                    catch(Exception)
                    {

                    }

                    int nivo = korisnikovCenovniUslovZaOvajProizvod == null ? 0 : korisnikovCenovniUslovZaOvajProizvod.Item2;

                    double K =  razlika / (Program.nProfiCenovnikNivoa - 1) * nivo;

                    if (minCena <= 0 || maxCena <= 0)
                        c.Add(new Artikal() { ID = proizvod.RobaID, Cena = new Cena() { VPCena = -999999, PDV = proizvod.PDV / 100 } });
                    else
                    {
                        double cenaZaKupca = maxCena - K;

                        // Dozvoljava max 15% rabata
                        if ((((cenaZaKupca / maxCena) - 1) * -100) > 15)
                            cenaZaKupca = maxCena * 0.85;

                        c.Add(new Artikal() { ID = proizvod.RobaID, Cena = new Cena() { VPCena = cenaZaKupca, PDV = proizvod.PDV / 100 } });
                    }
                }

                return c;
            }
            /// <summary>
            /// Vraca listu svih cenovnika za sve korisnike
            /// </summary>
            /// <returns></returns>
            public static List<Cenovnik> List()
            {
                List<Cenovnik> list = new List<Cenovnik>();
                using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                {
                    con.Open();
                    List<Korisnik> korisnici = new List<Korisnik>(Korisnik.List());
                    foreach (Korisnik k in korisnici)
                    {
                        Cenovnik c = new Cenovnik();
                        c.KorisnikID = k.ID;
                        List<Proizvod> sviProizvodi = Proizvod.List();

                        // CenovnaGrupaID / nivo za korisnika
                        List<Tuple<int, int>> uslovi = new List<Tuple<int, int>>();

                        uslovi.AddRange(GetNivoeZaGrupuID(con, k.ID));

                        foreach (Proizvod proizvod in sviProizvodi)
                        {
                            double minCena = proizvod.NabavnaCena;
                            double maxCena = proizvod.ProdajnaCena;
                            double r = maxCena - minCena;
                            double razlika = r * (1 - OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH);

                            Tuple<int, int> korisnikovCenovniUslovZaOvajProizvod = null;
                            try
                            {
                                korisnikovCenovniUslovZaOvajProizvod = uslovi.Where(x => x.Item1 == proizvod.CenovnaGrupaID).FirstOrDefault();
                            }
                            catch (Exception)
                            {

                            }

                            int nivo = korisnikovCenovniUslovZaOvajProizvod == null ? 0 : korisnikovCenovniUslovZaOvajProizvod.Item2;

                            double K = razlika / (Program.nProfiCenovnikNivoa - 1) * nivo;
                            
                            if (minCena <= 0 || maxCena <= 0)
                                c.Add(new Artikal() { ID = proizvod.RobaID, Cena = new Cena() { VPCena = -999999 } });
                            else
                            {
                                double cenaZaKupca = maxCena - K;

                                // Dozvoljava max 15% rabata
                                if ((((cenaZaKupca / maxCena) - 1) * -100) > 15)
                                    cenaZaKupca = maxCena * 0.85;

                                c.Add(new Artikal() { ID = proizvod.RobaID, Cena = new Cena() { VPCena = cenaZaKupca, PDV = proizvod.PDV / 100 } });
                            }
                        }
                    }
                }

                return list;
            }
            /// <summary>
            /// Vraca cenovnik za odredjeni cenovni nivo (Platinum = 3, Gold = 2, Silver = 1, Iron = 0)
            /// </summary>
            /// <param name="cenovniNivo">Platinum = 3, Gold = 2, Silver = 1, Iron = 0</param>
            /// <returns></returns>
            public static Cenovnik GetZaNivo(int cenovniNivo)
            {
                Cenovnik c = new Cenovnik();
                c.KorisnikID = -1;

                List<Proizvod> sviProizvodi = Proizvod.List();
                List<CenovnikGrupa> grupeCenovnika = CenovnikGrupa.List();
                List<Tuple<int, int>> uslovi = new List<Tuple<int, int>>();

                foreach(CenovnikGrupa gc in grupeCenovnika)
                    uslovi.Add(new Tuple<int, int>(gc.ID, cenovniNivo));

                foreach (Proizvod proizvod in sviProizvodi)
                {
                    double minCena = proizvod.NabavnaCena;
                    double maxCena = proizvod.ProdajnaCena;
                    double r = maxCena - minCena;
                    double razlika = r * (1 - OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH);

                    if(proizvod.RobaID == 332)
                    {
                        var a = 1;
                    }
                    Tuple<int, int> korisnikovCenovniUslovZaOvajProizvod = null;
                    try
                    {
                        korisnikovCenovniUslovZaOvajProizvod = uslovi.Where(x => x.Item1 == proizvod.CenovnaGrupaID).FirstOrDefault();
                    }
                    catch (Exception)
                    {

                    }

                    int nivo = korisnikovCenovniUslovZaOvajProizvod == null ? 0 : korisnikovCenovniUslovZaOvajProizvod.Item2;
                    double K = razlika / (Program.nProfiCenovnikNivoa - 1) * nivo;

                    if (minCena <= 0 || maxCena <= 0)
                        c.Add(new Artikal() { ID = proizvod.RobaID, Cena = new Cena() { VPCena = -999999 } });
                    else
                    {
                        double cenaZaKupca = maxCena - K;

                        // Dozvoljava max 15% rabata
                        if ((((cenaZaKupca / maxCena) - 1) * -100) > 15)
                            cenaZaKupca = maxCena * 0.85;

                        c.Add(new Artikal() { ID = proizvod.RobaID, Cena = new Cena() { VPCena = cenaZaKupca, PDV = proizvod.PDV / 100 } });
                    }
                }
                return c;
            }
            /// <summary>
            /// Vraca cenovnik za odredjeni cenovni nivo (Platinum = 3, Gold = 2, Silver = 1, Iron = 0)
            /// </summary>
            /// <param name="cenovniNivo">Platinum = 3, Gold = 2, Silver = 1, Iron = 0</param>
            /// <returns></returns>
            public static Task<Cenovnik> GetZaNivoAsync(int cenovniNivo)
            {
                return Task.Run(() =>
                {
                    return GetZaNivo(cenovniNivo);
                });
            }


            /// <summary>
            /// Ne znam da li treba ovde da stoji
            /// </summary>
            /// <param name="con"></param>
            /// <param name="userID"></param>
            /// <returns></returns>
            private static List<Tuple<int, int>> GetNivoeZaGrupuID(MySqlConnection con, int userID)
            {
                List<Tuple<int, int>> list = new List<Tuple<int, int>>();
                using (MySqlCommand cmd = new MySqlCommand("SELECT NIVO, CENOVNIK_GRUPAID FROM USER_CENOVNIK WHERE USERID = @UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", userID);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Tuple<int, int>(Convert.ToInt32(dr["CENOVNIK_GRUPAID"]), Convert.ToInt32(dr["NIVO"])));
                }

                return list;
            }
        }
    }
}
