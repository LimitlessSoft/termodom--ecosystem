using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System.Text;

namespace TDBrain_v3
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class BigBrain
    {
        private static Task? _mainLoop { get; set; }
        private static TimeSpan _mainLoopTimeout { get; set; } = TimeSpan.FromSeconds(1);
        private static Task? _smsCheckupLoop { get; set; }
        private static TimeSpan _smsCheckTimeout { get; set; } = TimeSpan.FromMinutes(30);
        private static bool _running { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public static bool Running { get => _running; }

        /// <summary>
        /// Starts and maintain main loop
        /// </summary>
        /// <returns></returns>
        private static Task MainLoop_StartAsync()
        {
            return Task.Run(async () =>
            {
                Debug.Log("Startujem main loop");
                while (_running)
                {
                    await UpdateAsync();
                    Thread.Sleep(_mainLoopTimeout);
                }
            });
        }
        /// <summary>
        /// Update method that is ran each second
        /// </summary>
        private static async Task UpdateAsync()
        {
#if !DEBUG
            await ObradiWebAkcijeAsync();
#endif
        }
        /// <summary>
        /// Starts and maintain sms checkup loop
        /// </summary>
        /// <returns></returns>
        private static Task SmsCheckupLoop_StartAsync()
        {
            return Task.Run(async () =>
            {
                while(_running)
                {
                    await SmsCheckupLoop_UpdateAsync();
                    Thread.Sleep(_smsCheckTimeout);
                }
            });
        }
        /// <summary>
        /// Update method that is ran each [_smsCheckTimeout]
        /// </summary>
        /// <returns></returns>
        private static async Task SmsCheckupLoop_UpdateAsync()
        {
            await Task.Run(() =>
            {
                GSMModem.QueueSMS(null, $"SMS Server Check. Next check sceduled for:" +
                    $"{DateTime.Now.Add(_smsCheckTimeout).ToString("dd.MM.yyyy (HH:mm)")}");
            });
        }

        /// <summary>
        /// Startuje big brain loop koji radi sve procese u pozadini.
        /// Ukoliko je loop vec startovan, gledace parametar 'restartIfStarted' da zna da li da restartuje ili ne.
        /// </summary>
        public static void Start()
        {
            Debug.Log("Startujem Big Brain...");
            if(_smsCheckupLoop == null)
                _smsCheckupLoop = SmsCheckupLoop_StartAsync();

            if(_mainLoop != null)
                throw new Exception("Proces je vec startovan!");

            _mainLoop = MainLoop_StartAsync();
        }
        private static async Task ObradiWebAkcijeAsync()
        {
            try
            {
                var resp = await TDWebAPI.GetAsync("/api/akc/list");
                var r = await resp.Content.ReadAsStringAsync();
                List<AKCDTO>? akcs = JsonConvert.DeserializeObject<List<AKCDTO>>(r);

                if (akcs == null || akcs.Count == 0)
                    return;

                foreach (AKCDTO akc in akcs)
                {
                    Debug.Log("======================");
                    try
                    {
                        string[] parts = akc.Action.Split('|');
                        switch (parts[0])
                        {
                            case "SENDSMS":
                                try
                                {
                                    Debug.Log(" WEB: Pokrenuta je akcija slanja sms-a sa tekstom '" + parts[2] + "' na broj " + parts[1] + "!");
                                    GSMModem.QueueSMS(new List<string>() { parts[1] }, parts[2]);
                                }
                                catch(Exception ex)
                                {
                                    Debug.Log($"Greska prilikom slanja SMS-a!");
                                    Debug.Log(ex.ToString());
                                }
                                break;
                            case "PretvoriUProracun":
                                try
                                {
                                    await PretvoriUProracun(Convert.ToInt32(parts[1]));
                                }
                                catch(Exception ex)
                                {
                                    Debug.Log($"Greska prilikom pretvaranja u proracun!");
                                    Debug.Log(ex.ToString());
                                }
                                break;
                            default:
                                Debug.Log("Nepoznata akcija!");
                                Debug.Log(akc.Action.ToString());
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"Nepoznata greska!");
                        Debug.Log(ex.ToString());
                    }

                    try
                    {
                        Debug.Log("Uklanjam zadatak sa Web-a!");
                        await TDWebAPI.PostAsync($"/api/akc/delete/{akc.ID}");
                        Debug.Log("Zadatak izvrsen!");
                        Debug.Log("======================");
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Greska prilikom uklanjanja akcije sa web-a!");
                        Debug.Log(ex.ToString());
                        Debug.Log("======================");
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log(ex.ToString());
            }
        }
        private static async Task PretvoriUProracun(int brPorudzbine)
        {
            try
            {
                Debug.Log("Pretvaranje u proracun zapoceto");
                Debug.Log("Porudzbina ID: " + brPorudzbine);
                PorudzbinaDTO? porudzbina = JsonConvert.DeserializeObject<PorudzbinaDTO>(await (await TDWebAPI.GetAsync($"/webshop/porudzbina/get?id={brPorudzbine}")).Content.ReadAsStringAsync());
                var stavkePorudzbineResp = await TDWebAPI.GetAsync($"/webshop/stavka/list?porudzbinaID={brPorudzbine}");
                List<StavkaDTO>? stavke = JsonConvert.DeserializeObject<List<StavkaDTO>>(await stavkePorudzbineResp.Content.ReadAsStringAsync());

                if (porudzbina == null)
                {
                    Debug.Log($"Porudzbina sa IDem {brPorudzbine} nije pronadjena!");
                    return;
                }

                if (stavke == null || stavke.Count == 0)
                {
                    Debug.Log($"Porudzbina sa IDem {brPorudzbine} je prazna!");
                    return;
                }

                if (porudzbina.NacinPlacanja != 1)
                    porudzbina.NacinPlacanja = 5;

                using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[porudzbina.MagacinID + 100, DateTime.Now.Year]))
                {
                    con.Open();

                    int brDokKom = DB.Komercijalno.DokumentManager.Insert(con, 32, "WEB " + brPorudzbine, null,
                        "WEB " + brPorudzbine, porudzbina.NacinPlacanja, porudzbina.MagacinID + 100, null, null);

                    DB.Komercijalno.DokumentManager? dokument = DB.Komercijalno.DokumentManager.Get(con, 32, brDokKom);
                    if (dokument == null)
                    {
                        Debug.Log($"Dokument proracuna koji je upravo kreiran nije pronadjen!");
                        return;
                    }

                    dokument.AliasU = porudzbina.KorisnikID;
                    dokument.OpisUpl = porudzbina.ImeIPrezime;
                    dokument.Update(con);

                    List<Termodom.Data.Entities.Komercijalno.Roba> roba = DB.Komercijalno.RobaManager.Collection(con).Values.ToList();
                    List<DB.Komercijalno.RobaUMagacinuManager> rums = DB.Komercijalno.RobaUMagacinuManager.List(con);
                    var tarife = Managers.Komercijalno.TarifeManager.Dictionary(con);
                    foreach (var stavka in stavke)
                    {
                        Termodom.Data.Entities.Komercijalno.Roba? rob = roba.FirstOrDefault(x => x.ID == stavka.RobaID);
                        DB.Komercijalno.RobaUMagacinuManager? rum = rums.FirstOrDefault(x => x.RobaID == stavka.RobaID);

                        if (rob == null)
                        {
                            Debug.Log($"Roba sa datim ID-om ({stavka.RobaID}) nije pronadjena!");
                            break;
                        }

                        if (rum == null)
                        {
                            Debug.Log($"Roba u magacinu sa datim ID-om ({stavka.RobaID}) nije pronadjen!");
                            break;
                        }

                        Termodom.Data.Entities.Komercijalno.Tarifa tarifa = tarife[rob.TarifaID];

                        if (tarifa == null)
                        {
                            Debug.Log($"Tarifa za datu robu nije pronadjena!");
                            break;
                        }

                        double stopa = Convert.ToDouble(tarifa.Stopa);

                        double prodajnaCenaNaDan = DB.Komercijalno.ProcedureManager.ProdajnaCenaNaDan(con, porudzbina.MagacinID + 100, stavka.RobaID, DateTime.Now);
                        double pcNaDanBezPDV = prodajnaCenaNaDan * (1 - (stopa / (100 + stopa)));

                        double rabat = (1 - (stavka.VpCena / pcNaDanBezPDV)) * 100;

                        DB.Komercijalno.StavkaManager.Insert(con, dokument, rob, rum, stavka.Kolicina, Math.Max(0, rabat));
                    }

                    StringBuilder komentar = new StringBuilder();
                    komentar.AppendLine(porudzbina.KontaktMobilni);
                    komentar.AppendLine("Adresa isporuke: " + porudzbina.AdresaIsporuke);
                    komentar.AppendLine("Komentar kupac:" + porudzbina.Napomena);
                    komentar.AppendLine("============");
                    komentar.AppendLine(porudzbina.KomercijalnoKomentar);
                    DB.Komercijalno.KomentariManager.Insert(con, dokument.VrDok, dokument.BrDok, komentar.ToString(), porudzbina.KomercijalnoInterniKomentar, "");

                    var res = await TDWebAPI.PostAsync($"/webshop/porudzbina/brdokkomercijalno/set?porudzbinaid={brPorudzbine}&brDokKomercijalno={brDokKom}");

                    if ((int)res.StatusCode != 200)
                        Debug.Log("Greska prilikom azuriranja broja dokumenta komercijalnog poslovanja u porudzbini na web-u!");

                    res = await TDWebAPI.PostAsync($"/webshop/porudzbina/status/set?porudzbinaid={brPorudzbine}&status=1");

                    if ((int)res.StatusCode != 200)
                        Debug.Log("Greska prilikom azuriranja statusa porudzbine na web-u!");

                    Debug.Log("Porudzbina pretvorena u dokument proracuna broj " + brDokKom);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }

    public static partial class BigBrain
    {
        private class AKCDTO
        {
            public int ID { get; set; }
            public string? Action { get; set; }
            public DateTime Date { get; set; }
            public int Sender { get; set; }
        }

        private class PorudzbinaDTO
        {
            public int ID { get; set; }
            public int KorisnikID { get; set; }
            public int BrDokKomercijalno { get; set; }
            public DateTime Datum { get; set; }
            public int Status { get; set; }
            public int MagacinID { get; set; }
            public int? PPID { get; set; }
            public string? InterniKomentar { get; set; }
            public int? ReferentObrade { get; set; }
            public int NacinPlacanja { get; set; }
            public string? Hash { get; set; }
            public double K { get; set; }
            public double UstedaKorisnik { get; set; }
            public double UstedaKlijent { get; set; }
            public int Dostava { get; set; }
            public string? KomercijalnoInterniKomentar { get; set; }
            public string? KomercijalnoKomentar { get; set; }
            public string? KomercijalnoNapomena { get; set; }
            public string? Napomena { get; set; }
            public string? AdresaIsporuke { get; set; }
            public string? KontaktMobilni { get; set; }
            public string? ImeIPrezime { get; set; }
        }
        private class StavkaDTO
        {
            public int ID { get; set; }
            public int PorudzbinaID { get; set; }
            public int RobaID { get; set; }
            public double Kolicina { get; set; }
            public double VpCena { get; set; }
            public double Rabat { get; set; }
        }
    }

}
