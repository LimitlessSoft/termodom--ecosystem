using Microsoft.Extensions.Logging;
using TD.Core.Contracts.IManagers;
using TD.Core.Domain.Managers;
using TD.Core.Framework.Extensions;
using TD.WebshopListener.Contracts.Constants;
using TD.WebshopListener.Contracts.Dtos;
using TD.WebshopListener.Contracts.IManagers;
using TD.WebshopListener.Contracts.Requests.Work;

namespace TD.WebshopListener.Domain.Managers
{
    public class WorkManager : IWorkManager
    {
        private readonly ILogger _logger;
        private readonly WebshopApiManager _webshopApiManager;
        private readonly TDBrainApiManager _tdBrainApiManager;
        private readonly ITaskSchedulerManager _taskSchedulerManager;

        public WorkManager(ILogger<WorkManager> logger, WebshopApiManager webshopApiManager, TDBrainApiManager tdBrainApiManager, ITaskSchedulerManager taskSchedulerManager)
        {
            _logger = logger;
            _webshopApiManager = webshopApiManager;
            _tdBrainApiManager = tdBrainApiManager;
            _taskSchedulerManager = taskSchedulerManager;
        }

        public void PretvoriUProracun(int porudzbinaId)
        {
            var porudzbinaResponse = _webshopApiManager.GetAsync<PorudzbinaDto>($"/webshop/porudzbina/get?id={porudzbinaId}").GetAwaiter().GetResult();
            if(porudzbinaResponse.NotOk)
            {
                _logger.LogError($"Error getting response from /webshop/porudzbina/get?id={porudzbinaId}!");
                return;
            }

            var stavkeResponse = _webshopApiManager.GetAsync<List<StavkaDto>>($"/webshop/stavka/list?porudzbinaID={porudzbinaId}").GetAwaiter().GetResult();
            if (stavkeResponse.NotOk)
            {
                _logger.LogError($"Error getting response from /webshop/stavka/list?porudzbinaID={porudzbinaId}!");
                return;
            }

            var porudzbina = porudzbinaResponse.Payload;
            if (porudzbina == null)
            {
                _logger.LogError($"Porudzbina sa id-em {porudzbinaId} nije pronadjena!");
                return;
            }

            var stavke = stavkeResponse.Payload;
            if (stavke == null || stavke.Count == 0)
            {
                _logger.LogInformation($"Porudzbina sa id-em {porudzbinaId} je praznaa!");
                return;
            }

            if (porudzbina.NacinPlacanja != 1)
                porudzbina.NacinPlacanja = 5;

            var inserDokumentResponse = _tdBrainApiManager.PostAsync<TDBrainDokumentInsertRequest>("/komercijalno/dokument/insert", new TDBrainDokumentInsertRequest()
            {
                BazaId = 112,
                DozvoliDaljeIzmeneUKomercijalnom = true,
                GodinaBaze = 2023,
                InterniBroj = "123",
                KomercijalnoKorisnikId = 111,
                MagacinId = 112,
                NuId = 5,
                VrDok = 32
            }).GetAwaiter().GetResult();

            //using (FbConnection con = new FbConnection("data source=192.168.0.3; initial catalog = E:\\4monitor\\Poslovanje\\Baze\\TDOffice_v2\\TDOffice_v2_2021.FDB; user=SYSDBA; password=masterkey"))
            //{
            //    con.Open();

            //    int brDokKom = DB.Komercijalno.DokumentManager.Insert(con, 32, "WEB " + brPorudzbine, null,
            //        "WEB " + brPorudzbine, porudzbina.NacinPlacanja, porudzbina.MagacinID + 100, null, null);

            //    DB.Komercijalno.DokumentManager? dokument = DB.Komercijalno.DokumentManager.Get(con, 32, brDokKom);
            //    if (dokument == null)
            //    {
            //        Debug.Log($"Dokument proracuna koji je upravo kreiran nije pronadjen!");
            //        return;
            //    }

            //    dokument.AliasU = porudzbina.KorisnikID;
            //    dokument.OpisUpl = porudzbina.ImeIPrezime;
            //    dokument.Update(con);

            //    List<Termodom.Data.Entities.Komercijalno.Roba> roba = DB.Komercijalno.RobaManager.Collection(con).Values.ToList();
            //    List<DB.Komercijalno.RobaUMagacinuManager> rums = DB.Komercijalno.RobaUMagacinuManager.List(con);
            //    var tarife = Managers.Komercijalno.TarifeManager.Dictionary(con);
            //    foreach (var stavka in stavke)
            //    {
            //        Termodom.Data.Entities.Komercijalno.Roba? rob = roba.FirstOrDefault(x => x.ID == stavka.RobaID);
            //        DB.Komercijalno.RobaUMagacinuManager? rum = rums.FirstOrDefault(x => x.RobaID == stavka.RobaID);

            //        if (rob == null)
            //        {
            //            Debug.Log($"Roba sa datim ID-om ({stavka.RobaID}) nije pronadjena!");
            //            break;
            //        }

            //        if (rum == null)
            //        {
            //            Debug.Log($"Roba u magacinu sa datim ID-om ({stavka.RobaID}) nije pronadjen!");
            //            break;
            //        }

            //        Termodom.Data.Entities.Komercijalno.Tarifa tarifa = tarife[rob.TarifaID];

            //        if (tarifa == null)
            //        {
            //            Debug.Log($"Tarifa za datu robu nije pronadjena!");
            //            break;
            //        }

            //        double stopa = Convert.ToDouble(tarifa.Stopa);

            //        double prodajnaCenaNaDan = DB.Komercijalno.ProcedureManager.ProdajnaCenaNaDan(con, porudzbina.MagacinID + 100, stavka.RobaID, DateTime.Now);
            //        double pcNaDanBezPDV = prodajnaCenaNaDan * (1 - (stopa / (100 + stopa)));

            //        double rabat = (1 - (stavka.VpCena / pcNaDanBezPDV)) * 100;

            //        DB.Komercijalno.StavkaManager.Insert(con, dokument, rob, rum, stavka.Kolicina, Math.Max(0, rabat));
            //    }

            //    StringBuilder komentar = new StringBuilder();
            //    komentar.AppendLine(porudzbina.KontaktMobilni);
            //    komentar.AppendLine("Adresa isporuke: " + porudzbina.AdresaIsporuke);
            //    komentar.AppendLine("Komentar kupac:" + porudzbina.Napomena);
            //    komentar.AppendLine("============");
            //    komentar.AppendLine(porudzbina.KomercijalnoKomentar);
            //    DB.Komercijalno.KomentariManager.Insert(con, dokument.VrDok, dokument.BrDok, komentar.ToString(), porudzbina.KomercijalnoInterniKomentar, "");

            //    var res = await TDWebAPI.PostAsync($"/webshop/porudzbina/brdokkomercijalno/set?porudzbinaid={brPorudzbine}&brDokKomercijalno={brDokKom}");

            //    if ((int)res.StatusCode != 200)
            //        Debug.Log("Greska prilikom azuriranja broja dokumenta komercijalnog poslovanja u porudzbini na web-u!");

            //    res = await TDWebAPI.PostAsync($"/webshop/porudzbina/status/set?porudzbinaid={brPorudzbine}&status=1");

            //    if ((int)res.StatusCode != 200)
            //        Debug.Log("Greska prilikom azuriranja statusa porudzbine na web-u!");

            //    Debug.Log("Porudzbina pretvorena u dokument proracuna broj " + brDokKom);
            //}
        }

        public Task StartListeningWebshopAkcAsync()
        {
            _taskSchedulerManager.AddTask(new Core.Contracts.Tasks.Task(() =>
            {
                var resp = _webshopApiManager.GetAsync<List<AkcDto>>(WebApiEndpoints.GetAkcList()).GetAwaiter().GetResult();

                if (resp == null || resp.Payload == null || resp.Payload.IsEmpty())
                    return;

                foreach (var akc in resp.Payload)
                {
                    string[] parts = akc.Action.Split('|');
                    switch (parts[0])
                    {
                        case "SENDSMS":
                            _logger.LogError(Contracts.Messages.CommonMessages.ActionNotHandledMessage(parts[0]));
                            break;
                        case "PretvoriUProracun":
                            int porudzbinaId;

                            if (!int.TryParse(parts[1], out porudzbinaId))
                                _logger.LogError($"Could not parse '{porudzbinaId}' into '{nameof(porudzbinaId)}'");

                            PretvoriUProracun(porudzbinaId);
                            break;
                        default:
                            _logger.LogError(Contracts.Messages.CommonMessages.ActionNotHandledMessage(parts[0]));
                            break;
                    }
                }
            }, TimeSpan.FromSeconds(10)));
            return _taskSchedulerManager.RunTasksAsync(true);
        }
    }
}
