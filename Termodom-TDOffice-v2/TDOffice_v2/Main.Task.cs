using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    partial class Main
    {
        private static readonly TimeSpan TASK_CHECKER_LOOP_INTERVAL = TimeSpan.FromMinutes(1);
        private Task taskCheckerLoop { get; set; } = null;
        /// <summary>
        /// Asinhrono pokrece loop koji prolazi kroz sve taskove koje korisnik ima zadate i pokrece ih ukoliko ih korisnik nije izvrsio u datom intervalu.
        /// </summary>
        /// <returns></returns>
        private void StartTaskCheckerLoopAsync()
        {
            if (taskCheckerLoop != null && taskCheckerLoop.Status == TaskStatus.Running)
                throw new Exception("Task is already started!");

            taskCheckerLoop = Task.Run(async () => {
                while (Program.IsRunning)
                {
                    List<TDOffice.TDTaskItem> taskoviTrenutnogKorisnika = TDOffice.TDTaskItem.ListByUserID(Program.TrenutniKorisnik.ID);

                    foreach (TDOffice.TDTaskItem item in taskoviTrenutnogKorisnika)
                    {
                        if (item.Done != null && (DateTime.Now - (DateTime)item.Done).TotalMinutes < item.Interval)
                            continue;

                        switch ((TDOffice.TDTask)item.TDTaskID)
                        {
                            case TDOffice.TDTask.AutomatskeNivelacije:
                                PosaljiIzvestajAutomatskihNivelacija();
                                break;
                            case TDOffice.TDTask.NereseneKalkulacije: // Nezakljucane MP Kalkulacije - fali dokumentacija
                                PosaljiIzvestajNezakljucanihMPKalkulacijaKaoPoruku();
                                break;
                            case TDOffice.TDTask.ProveraMarzeProdajnogDokumenta: // Provera Marze Prodajnih Dokumenata
                                PosaljiIzvestajDokumenataUKojimaJeMarzaManjaOdProsecneKaoPoruku();
                                break;
                            case TDOffice.TDTask.AutomatskoGenerisanjePopisa: // Generisani Popis
                                Models.TDTask.GenerisiPopis(Program.TrenutniKorisnik.MagacinID, Program.TrenutniKorisnik.ID);
                                break;
                            case TDOffice.TDTask.ProveraDaLiImaPraznihFakturaUPoslednjih15Dana:
                                if (Komercijalno.Dokument.List("MAGACINID = 50 AND VRDOK = 13 AND FLAG = 0 AND DATUM >= '" + DateTime.Now.AddDays(15).ToString("dd.MM.yyyy") + "'").Count < 1)
                                    for (int i = 0; i < 2; i++)
                                        Komercijalno.Dokument.Insert(DateTime.Now.Year, 13, "TDAuto", null, null, (int)Komercijalno.NacinUplate.Virman, 50, null, 50);
                                break;
                            case TDOffice.TDTask.PrometMagacina:
                                List<int> magacini = new List<int>() { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
                                int godina = DateTime.Now.Year;
                                DataTable odobreniRabatiRezultat = Models.Izvestaj.OdobreniRabati(magacini, godina, new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                                TDOffice.Poruka.Insert(new TDOffice.Poruka()
                                {
                                    Datum = DateTime.Now,
                                    Naslov = "<TASK> Promet magacina",
                                    Posiljalac = Program.TrenutniKorisnik.ID,
                                    Primalac = Program.TrenutniKorisnik.ID,
                                    Status = TDOffice.PorukaTip.Standard,
                                    Tag = new TDOffice.PorukaAdditionalInfo()
                                    {
                                        Action = TDOffice.PorukaAction.OdobreniRabatMagacina,
                                        AdditionalInfo = odobreniRabatiRezultat
                                    },
                                    Tekst = string.Join(Environment.NewLine, " ", "<TASK> Promet Magacina. Pogledaj prilog gore desno!")
                                });
                                break;
                            case TDOffice.TDTask.MinimalneZalihe:
                                Task<List<Komercijalno.Roba>> roba = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
                                Task<List<Komercijalno.RobaUMagacinu>> robaUMagacinu = Komercijalno.RobaUMagacinu.ListAsync();
                                List<int> magaciniPP = new List<int>() { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };

                                TDOffice.Config<double> koeficijentConfig = TDOffice.Config<double>.Get(TDOffice.ConfigParameter.KoeficijentMinZalihe);
                                if (koeficijentConfig.Tag <= 0)
                                {
                                    MessageBox.Show($"Koeficijent u globalnim podseavanjima ima vrednost {koeficijentConfig.Tag} koja nije validna. Vrednost mora biti veca od 0!");
                                    return;
                                }

                                DataTable pt = Models.Izvestaj.ProdajaRobeZaDatiPeriod_Kolicinski(magaciniPP, DateTime.Now.AddMonths(-3), DateTime.Now);

                                DataTable finalDT = new DataTable();
                                finalDT.Columns.Add("MagacinID", typeof(int));
                                finalDT.Columns.Add("RobaID", typeof(int));
                                finalDT.Columns.Add("Naziv", typeof(string));
                                finalDT.Columns.Add("ProsecnaProdajaPoslednja3Meseca", typeof(double));
                                finalDT.Columns.Add("TrenutnoStanje", typeof(double));
                                finalDT.Columns.Add("JM", typeof(string));

                                foreach (DataRow dr in pt.Rows)
                                {
                                    int magacinID = Convert.ToInt32(dr["MagacinID"]);
                                    int robaID = Convert.ToInt32(dr["RobaID"]);
                                    double prosekProdaje = Convert.ToDouble(dr["Kolicina"]);

                                    Komercijalno.Roba r = roba.Result.FirstOrDefault(x => x.ID == robaID);
                                    Komercijalno.RobaUMagacinu rum = robaUMagacinu.Result.FirstOrDefault(x => x.MagacinID == magacinID && x.RobaID == robaID);

                                    if (rum == null || prosekProdaje < rum.Stanje * koeficijentConfig.Tag)
                                        continue;

                                    DataRow row = finalDT.NewRow();
                                    row["MagacinID"] = dr["MagacinID"];
                                    row["RobaID"] = dr["RobaID"];
                                    row["Naziv"] = r == null ? "UNDEFINED" : r.Naziv;
                                    row["ProsecnaProdajaPoslednja3Meseca"] = dr["Kolicina"];
                                    row["TrenutnoStanje"] = rum == null ? -1 : rum.Stanje;
                                    row["JM"] = r == null ? "UNDEFINED" : r.JM;
                                    finalDT.Rows.Add(row);
                                }
                                TDOffice.Poruka.Insert(new TDOffice.Poruka()
                                {
                                    Datum = DateTime.Now,
                                    Naslov = "<TASK> Izvestaj minimalnih zaliha",
                                    Posiljalac = Program.TrenutniKorisnik.ID,
                                    Primalac = Program.TrenutniKorisnik.ID,
                                    Status = TDOffice.PorukaTip.Standard,
                                    Tag = new TDOffice.PorukaAdditionalInfo()
                                    {
                                        Action = TDOffice.PorukaAction.DataTableAttachment,
                                        AdditionalInfo = finalDT
                                    },
                                    Tekst = string.Join(Environment.NewLine, " ", "<TASK> U prilogu se nalazi izvestaj minimalnih zaliha magacina. Pogledaj prilog gore desno!")
                                });
                                break;
                            case TDOffice.TDTask.PrekomerneZalihe:
                                Task<List<Komercijalno.Roba>> robapz = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
                                Task<List<Komercijalno.RobaUMagacinu>> robaUMagacinupz = Komercijalno.RobaUMagacinu.ListAsync();
                                List<int> magacinipz = new List<int>() { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
                                TDOffice.Config<double> koeficijentConfigpz = TDOffice.Config<double>.Get(TDOffice.ConfigParameter.KoeficijentPrekomernihZaliha);

                                if (koeficijentConfigpz.Tag <= 0)
                                {
                                    MessageBox.Show($"Koeficijent u globalnim podseavanjima ima vrednost {koeficijentConfigpz.Tag} koja nije validna. Vrednost mora biti veca od 0!");
                                    return;
                                }
                                DataTable pz = Models.Izvestaj.ProdajaRobeZaDatiPeriod_Kolicinski(magacinipz, DateTime.Now.AddMonths(-3), DateTime.Now);

                                DataTable finalDTpz = new DataTable();
                                finalDTpz.Columns.Add("MagacinID", typeof(int));
                                finalDTpz.Columns.Add("RobaID", typeof(int));
                                finalDTpz.Columns.Add("Naziv", typeof(string));
                                finalDTpz.Columns.Add("ProsecnaProdajaPoslednja3Meseca", typeof(double));
                                finalDTpz.Columns.Add("TrenutnoStanje", typeof(double));
                                finalDTpz.Columns.Add("JM", typeof(string));

                                foreach (DataRow dr in pz.Rows)
                                {
                                    int magacinID = Convert.ToInt32(dr["MagacinID"]);
                                    int robaID = Convert.ToInt32(dr["RobaID"]);
                                    double prosekProdaje = Convert.ToDouble(dr["Kolicina"]);

                                    Komercijalno.Roba r = robapz.Result.FirstOrDefault(x => x.ID == robaID);
                                    Komercijalno.RobaUMagacinu rum = robaUMagacinupz.Result.FirstOrDefault(x => x.MagacinID == magacinID && x.RobaID == robaID);

                                    if (rum == null || prosekProdaje >= rum.Stanje * koeficijentConfigpz.Tag)
                                        continue;

                                    DataRow row = finalDTpz.NewRow();
                                    row["MagacinID"] = dr["MagacinID"];
                                    row["RobaID"] = dr["RobaID"];
                                    row["Naziv"] = r == null ? "UNDEFINED" : r.Naziv;
                                    row["ProsecnaProdajaPoslednja3Meseca"] = dr["Kolicina"];
                                    row["TrenutnoStanje"] = rum == null ? -1 : rum.Stanje;
                                    row["JM"] = r == null ? "UNDEFINED" : r.JM;
                                    finalDTpz.Rows.Add(row);
                                }
                                TDOffice.Poruka.Insert(new TDOffice.Poruka()
                                {
                                    Datum = DateTime.Now,
                                    Naslov = "<TASK> Izvestaj prekomernih zaliha",
                                    Posiljalac = Program.TrenutniKorisnik.ID,
                                    Primalac = Program.TrenutniKorisnik.ID,
                                    Status = TDOffice.PorukaTip.Standard,
                                    Tag = new TDOffice.PorukaAdditionalInfo()
                                    {
                                        Action = TDOffice.PorukaAction.DataTableAttachment,
                                        AdditionalInfo = finalDTpz
                                    },
                                    Tekst = string.Join(Environment.NewLine, " ", "<TASK> U prilogu se nalazi izvestaj prekomernih zaliha magacina. Pogledaj prilog gore desno!")
                                });
                                break;
                            case TDOffice.TDTask.NedovrsenaRazduzenja:
                                List<TDOffice.DokumentRazduzenjaMagacina> dokumenti = TDOffice.DokumentRazduzenjaMagacina.List("STATUS = " + (int)TDOffice.DokumentRazduzenjaMagacinaStatus.Zakljucan);
                                List<Komercijalno.Magacin> magaciniNR = Komercijalno.Magacin.ListAsync().Result;
                                List<TDOffice.User> korisnici = TDOffice.User.List();
                                DataTable nedovrsenaRazduzenja = new DataTable();
                                nedovrsenaRazduzenja.Columns.Add("ID", typeof(int));
                                nedovrsenaRazduzenja.Columns.Add("Datum", typeof(DateTime));
                                nedovrsenaRazduzenja.Columns.Add("MagacinID", typeof(int));
                                nedovrsenaRazduzenja.Columns.Add("Magacin", typeof(string));
                                nedovrsenaRazduzenja.Columns.Add("Referent", typeof(string));
                                nedovrsenaRazduzenja.Columns.Add("Komentar", typeof(string));
                                nedovrsenaRazduzenja.Columns.Add("InterniKomentar", typeof(string));

                                foreach (TDOffice.DokumentRazduzenjaMagacina drz in dokumenti)
                                {
                                    Komercijalno.Magacin m = magaciniNR.FirstOrDefault(x => x.ID == drz.MagacinID);
                                    TDOffice.User k = korisnici.FirstOrDefault(x => x.ID == drz.KorisnikID);

                                    DataRow dr = nedovrsenaRazduzenja.NewRow();
                                    dr["ID"] = drz.ID;
                                    dr["Datum"] = drz.Datum;
                                    dr["MagacinID"] = drz.MagacinID;
                                    dr["Magacin"] = m == null ? "Undefined" : m.Naziv;
                                    dr["Referent"] = k == null ? "Undefined" : k.Username;
                                    dr["Komentar"] = drz.Komentar;
                                    dr["InterniKomentar"] = drz.InterniKomentar;

                                    nedovrsenaRazduzenja.Rows.Add(dr);
                                }
                                TDOffice.Poruka.Insert(new TDOffice.Poruka()
                                {
                                    Datum = DateTime.Now,
                                    Naslov = "<TASK> Izvestaj nedovrsenih razduzenja",
                                    Posiljalac = Program.TrenutniKorisnik.ID,
                                    Primalac = Program.TrenutniKorisnik.ID,
                                    Status = TDOffice.PorukaTip.Standard,
                                    Tag = new TDOffice.PorukaAdditionalInfo()
                                    {
                                        Action = TDOffice.PorukaAction.DataTableAttachment,
                                        AdditionalInfo = nedovrsenaRazduzenja
                                    },
                                    Tekst = string.Join(Environment.NewLine, " ", "<TASK> U prilogu se nalazi izvestaj nedovrsenih razduzenja magacina. Pogledaj prilog gore desno!")
                                });
                                break;
                            case TDOffice.TDTask.TimeKeeper:
                                var res = await TDBrain_v3.PostAsync("/Komercijalno/TimeKeeper/Update");
                                break;
                            case TDOffice.TDTask.ZaposleniUgovor:
                                {
                                    DateTime dtod = DateTime.Now.Date;
                                    DateTime dtdo = DateTime.Now.Date.AddDays(15);
                                    List<TDOffice.ZaposleniUgovorORadu> zuor = TDOffice.ZaposleniUgovorORadu.List("KRAJ_TRAJANJA  >= '" + dtod.Date.ToString("MM/dd/yyyy")+ "' AND KRAJ_TRAJANJA  <= '" + dtdo.Date.ToString("MM/dd/yyyy") + "'");
                                    DataTable zaposleniugovor = new DataTable();
                                    zaposleniugovor.Columns.Add("ID", typeof(int));
                                    zaposleniugovor.Columns.Add("ZAPOSLENI_ID", typeof(int));
                                    zaposleniugovor.Columns.Add("Zaposleni", typeof(string));
                                    zaposleniugovor.Columns.Add("Firma", typeof(int));
                                    zaposleniugovor.Columns.Add("FirmaNaziv", typeof(string));
                                    zaposleniugovor.Columns.Add("PocetakTrajanja", typeof(DateTime));
                                    zaposleniugovor.Columns.Add("KrajTrajanja", typeof(DateTime));
                                    foreach (TDOffice.ZaposleniUgovorORadu zu in zuor)
                                    {
                                        TDOffice.Zaposleni zap = TDOffice.Zaposleni.Get(zu.ZaposleniID);
                                        TDOffice.Firma f = TDOffice.Firma.Get(zu.Firma);
                                        DataRow dr = zaposleniugovor.NewRow();
                                        dr["ID"] = zu.ID;
                                        dr["ZAPOSLENI_ID"] = zu.ZaposleniID;
                                        dr["Zaposleni"] = zap.ToString();
                                        dr["Firma"] = zu.Firma;
                                        dr["FirmaNaziv"] = f.Naziv;
                                        dr["PocetakTrajanja"] = zu.PocetakTrajanja;
                                        dr["KrajTrajanja"] = zu.KrajTrajanja;

                                        zaposleniugovor.Rows.Add(dr);
                                    }
                                    TDOffice.Poruka.Insert(new TDOffice.Poruka()
                                    {
                                        Datum = DateTime.Now,
                                        Naslov = "<TASK> Izvestaj Ugovora o radu koji isticu u narednih 15 dana",
                                        Posiljalac = Program.TrenutniKorisnik.ID,
                                        Primalac = Program.TrenutniKorisnik.ID,
                                        Status = TDOffice.PorukaTip.Standard,
                                        Tag = new TDOffice.PorukaAdditionalInfo()
                                        {
                                            Action = TDOffice.PorukaAction.DataTableAttachment,
                                            AdditionalInfo = zaposleniugovor
                                        },
                                        Tekst = string.Join(Environment.NewLine, " ", "<TASK> U prilogu se nalazi izvestaj Ugovora o radu koji isticu u narednih 15 dana. Pogledaj prilog gore desno!")
                                    });
                                }
                                break;
                            case TDOffice.TDTask.KomercijaloParametri:
                                TDOffice.Config<Dictionary<string, string>> sablon = TDOffice.Config<Dictionary<string, string>>.Get(TDOffice.ConfigParameter.KomercijalnoParametriSablon);
                                List<DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO> list = new List<DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO>();
                                foreach (string key in sablon.Tag.Keys)
                                    list.Add(new DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO(key, sablon.Tag[key]));

                                fm_KomercijalnoParametri_Index.PodesiVrednostiParametaraPoSablonu(list);
                                break;
                        }
                        item.Done = DateTime.Now;
                        item.Update();
                    }
                    Thread.Sleep(TASK_CHECKER_LOOP_INTERVAL);
                }
            });
        }

        #region Tasks
        private void PosaljiIzvestajNezakljucanihMPKalkulacijaKaoPoruku()
        {
            List<string> output = new List<string>();

            output.Add("=======================");
            output.Add("Izvestaj nezakljucanih MP kalkulacija ( neisporucena dokumenta stovarista )");
            output.Add("=======================");
            for (int z = 0; z < 2; z++)
            {
                output.Add("");
                output.Add("");
                output.Add("================================");
                output.Add($"==== Pocetak  { DateTime.Now.Year - z }  =====");
                output.Add("================================");
                output.Add("");

                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year - z]))
                {
                    con.Open();

                    List<Komercijalno.Dokument> mpKalkulacije = Komercijalno.Dokument.ListByVrDok(con, 18);
                    mpKalkulacije.RemoveAll(x => x.Flag != 0);

                    for (int i = 0; i < mpKalkulacije.Count; i++)
                    {
                        List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.ListByDokument(con, mpKalkulacije[i].VrDok, mpKalkulacije[i].BrDok);
                        if (stavke.Count == 0)
                            mpKalkulacije.RemoveAt(i);
                    }

                    if (mpKalkulacije.Count == 0)
                    {
                        output.Add("Sve mp kalkulacije su zakljucane!");
                    }
                    else
                    {
                        output.Add("----------------------");
                        output.Add("[Magacin]: [ukupno nezakljucanih kalkulacija] --- [nezakljucane kalkulacije starije od 7 dana]");
                        output.Add("----------------------");
                        for (int i = 12; i <= 28; i++)
                            output.Add($"M{i}: {mpKalkulacije.Count(x => x.MagacinID == i)} --- {mpKalkulacije.Count(x => x.MagacinID == i && x.Datum.AddDays(7) < DateTime.Now)}");
                        output.Add("----------------------");
                    }
                }

                output.Add("");
                output.Add("================================");
                output.Add($"==== Kraj  { DateTime.Now.Year - z }  =====");
                output.Add("================================");
                output.Add("");
                output.Add("");
            }

            TDOffice.Poruka.Insert(new TDOffice.Poruka()
            {
                Datum = DateTime.Now,
                Naslov = "TASK > Nezakljucane MP Kalkulacije",
                Posiljalac = -1,
                Primalac = Program.TrenutniKorisnik.ID,
                Status = TDOffice.PorukaTip.Standard,
                Tag = new TDOffice.PorukaAdditionalInfo(),
                Tekst = string.Join(Environment.NewLine, output)
            });
        }
        public static void PosaljiIzvestajDokumenataUKojimaJeMarzaManjaOdProsecneKaoPoruku()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(170001))
            {
                TDOffice.Pravo.NematePravoObavestenje(170001);
                return;
            }
            if (Program.PokrenutIzvestajNelogicnihMarzi)
            {
                Task.Run(() =>
                {
                    MessageBox.Show($"Vec ste pokrenuli ovu akciju, sacekajte svoj izvestaj!\nTrenutni napredak: {Program.IzvestajNelogicnihMarziTrenutniStage} / {Program.IzvestajNelogicnihMarziMaxStage}");
                });
                return;
            }

            Program.PokrenutIzvestajNelogicnihMarzi = true;
            int dana = 5;
            List<string> output = new List<string>();
            List<Dokument> dokumentiKomercijalno = Komercijalno.Dokument.List("DATUM >= '" + DateTime.Now.AddDays(-dana).ToString("dd.MM.yyyy") + "' AND (VRDOK = 15 OR VRDOK = 13 OR VRDOK = 32)");

            // vrdok, brdok, ostvarena marza, prodajna vrednost, dati rabat
            List<Tuple<int, int, double, double, double>> marze = new List<Tuple<int, int, double, double, double>>();

            List<Dokument> dokumentiNabavke = Dokument.List($"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");
            List<Stavka> stavkeNabavke = Komercijalno.Stavka.List(DateTime.Now.Year, "MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");

            Program.IzvestajNelogicnihMarziMaxStage = dokumentiKomercijalno.Count(x => x.Potrazuje != 0);
            Program.IzvestajNelogicnihMarziTrenutniStage = 0;
            Parallel.ForEach(dokumentiKomercijalno.Where(x => x.Potrazuje != 0), dok =>
            {
                double realizovanaMarza = dok.OstvarenaMarza(dokumentiNabavke, stavkeNabavke, Dokument.OstvarenaMarzaTipNabavneCene.NaDanDokumenta);
                double rabat = ((dok.Potrazuje / dok.Duguje) - 1) * -100;
                marze.Add(new Tuple<int, int, double, double, double>(dok.VrDok, dok.BrDok, realizovanaMarza, dok.Potrazuje, rabat));
                Program.IzvestajNelogicnihMarziTrenutniStage++;
            });

            if (marze.Count != 0)
            {
                double ukupnaProdajnaVrednost = marze.Where(x => x.Item1 == 15 || x.Item1 == 13).Sum(x => x.Item4);
                double ukupnaNabavnaVerdnost = marze.Where(x => x.Item1 == 15 || x.Item1 == 13).Sum(x => x.Item4 - (x.Item4 * (x.Item3 / (100 + x.Item3))));
                double ukupanRUCProcenat = (ukupnaProdajnaVrednost / ukupnaNabavnaVerdnost - 1) * 100;

                //double prosecnaMarza = marze.Where(x => x.Item1 == 15 || x.Item1 == 13).Sum(x => x.Item3) / marze.Count();

                output.Add($"Realizovana RUC u validnim dokumentima (15, 13) u proteklih {dana} dana je {ukupanRUCProcenat.ToString("0.00")}%");
                output.Add($"K = RUC - 30% = {(ukupanRUCProcenat * 0.7).ToString("0.00")}%");
                output.Add("Ispod je lista dokumenata cija je marza manja od toga");

                ukupanRUCProcenat = ukupanRUCProcenat * 0.7;
                output.Add("==============");
                output.Add("Pozitivne marze:");
                foreach (var m in marze.Where(x => x.Item3 >= 0 && x.Item3 < ukupanRUCProcenat && x.Item5 != 0))
                    output.Add($"VRDOK: {m.Item1} - BRDOK: {m.Item2} - MARZA:  {m.Item3.ToString("0.00\\%")} - RABAT: {m.Item5.ToString("0.00\\%")}");

                output.Add("==============");
                output.Add("Negativne marze:");

                foreach (var m in marze.Where(x => x.Item3 < 0 && x.Item5 != 0))
                    output.Add($"VRDOK: {m.Item1} - BRDOK: {m.Item2} - MARZA:  {m.Item3.ToString("0.00\\%")} - RABAT: {m.Item5.ToString("0.00\\%")}");
            }
            TDOffice.Poruka.Insert(new TDOffice.Poruka()
            {
                Datum = DateTime.Now,
                Naslov = "TASK > Nelogicne marze",
                Posiljalac = -1,
                Primalac = Program.TrenutniKorisnik.ID,
                Status = TDOffice.PorukaTip.Standard,
                Tag = new TDOffice.PorukaAdditionalInfo(),
                Tekst = output.Count == 0 ? "Nema" : string.Join(Environment.NewLine, output)
            });
            Program.PokrenutIzvestajNelogicnihMarzi = false;
        }
        private void PosaljiIzvestajAutomatskihNivelacija()
        {
            throw new NotImplementedException();
            List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.List(DateTime.Now.Year);

            foreach(Komercijalno.Stavka s in stavke)
            {
            }
        }
        #endregion
    }
}
