using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Extensions;
using TD.Core.Contracts.IManagers;
using TD.Core.Framework.Extensions;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.WebshopListener.Contracts.Constants;
using TD.WebshopListener.Contracts.Dtos;
using TD.WebshopListener.Contracts.IManagers;

namespace TD.WebshopListener.Domain.Managers
{
    public class WorkManager : IWorkManager
    {
        private readonly ILogger _logger;
        private readonly IWebshopApiManager _webshopApiManager;
        private readonly ITaskSchedulerManager _taskSchedulerManager;
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;

        public WorkManager(ILogger<WorkManager> logger,
            IWebshopApiManager webshopApiManager,
            IKomercijalnoApiManager komercijalnoApiManager,
            ITaskSchedulerManager taskSchedulerManager)
        {
            _logger = logger;
            _webshopApiManager = webshopApiManager;
            _taskSchedulerManager = taskSchedulerManager;
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public void PretvoriUDokument(int porudzbinaId, short vrDok)
        {
            var porudzbinaResponse = _webshopApiManager.GetRawAsync<PorudzbinaDto>($"/webshop/porudzbina/get?id={porudzbinaId}").GetAwaiter().GetResult();
            if (porudzbinaResponse.NotOk)
            {
                _logger.LogError($"Error getting response from /webshop/porudzbina/get?id={porudzbinaId}!");
                return;
            }

            var stavkeResponse = _webshopApiManager.GetRawAsync<List<StavkaDto>>($"/webshop/stavka/list?porudzbinaID={porudzbinaId}").GetAwaiter().GetResult();
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


            // TODO: Ovo izmeniti, mapirati magacine na sajtu kako treba
            porudzbina.MagacinID += 100;
            var insertDokumentResponse = _komercijalnoApiManager.PostAsync<DokumentCreateRequest, DokumentDto>("/dokumenti", new DokumentCreateRequest()
            {
                IntBroj = $"WEB {porudzbinaId}",
                ZapId = 1,
                RefId = 1,
                MagId = porudzbina.MagacinID,
                MagacinId = porudzbina.MagacinID,
                NuId = vrDok == 34 ? (short)18 : porudzbina.NacinPlacanja, // 18 = utovar, pomeriti u konstantu
                VrDok = vrDok,
                AliasU = (short)porudzbina.KorisnikID,
                OpisUpl = porudzbina.ImeIPrezime,
                NrId = 1,
                DodPorez = 0,
                PPID = vrDok == 34 ? 649 : porudzbina.PPID // 649 = NN, pomeriti u konstantu
            }).GetAwaiter().GetResult();

            if (insertDokumentResponse.NotOk || insertDokumentResponse.Payload == null)
            {
                _logger.LogError($"Error inserting dokument komercijalno Api POST /dokumenti!");
                return;
            }

            List<string> javniKomentar = new List<string>()
            {
                "Porudzbina kreirana uz pomoc www.termodom.rs profi kutka.",
                $"PorudzbinaId: {porudzbina.ID}",
                "",
                $"=========================",
                ""
            };

            if(!string.IsNullOrWhiteSpace(porudzbina.InterniKomentar))
            {
                javniKomentar.Add("Kupac je ostavio komentar:");
                javniKomentar.Add(porudzbina.InterniKomentar);
            }

            if(!string.IsNullOrWhiteSpace(porudzbina.KomercijalnoKomentar))
            {
                javniKomentar.Add("");
                javniKomentar.Add("==================");
                javniKomentar.Add("");
                javniKomentar.Add("Admin komentar:");
                javniKomentar.Add(porudzbina.KomercijalnoKomentar);
            }

            List<string> interniKomentar = new List<string>()
            {
                $"KupacId: {porudzbina.KorisnikID}",
                $"Kupac korisnicko ime: ToDo",
                $"Kupac ostavio kontakt: {porudzbina.KontaktMobilni}",
                $"Datum porucivanja: {porudzbina.Datum.ToString("dd.MM.yyyy")}",
                $"Datum obrade: {DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm")}"
            };

            if (!string.IsNullOrWhiteSpace(porudzbina.KomercijalnoInterniKomentar))
            {
                interniKomentar.Add("");
                interniKomentar.Add("==================");
                interniKomentar.Add("");
                interniKomentar.Add("Admin interni komentar:");
                interniKomentar.Add(porudzbina.KomercijalnoInterniKomentar);
            }

            List<string> privatniKomentar = new List<string>();

            var insertKomentarResponse = _komercijalnoApiManager.PostAsync<CreateKomentarRequest, KomentarDto>("/komentari", new CreateKomentarRequest()
            {
                VrDok = vrDok,
                BrDok = insertDokumentResponse.Payload.BrDok,
                Komentar = string.Join("\r\n", javniKomentar),
                InterniKomentar = string.Join("\r\n", interniKomentar),
                PrivatniKomentar = string.Join("\r\n", privatniKomentar)
            }).GetAwaiter().GetResult();

            if (insertKomentarResponse.NotOk)
            {
                _logger.LogError($"Error inserting komentar komercijalno Api POST /komentari!");
                return;
            }

            foreach (var stavka in stavke)
            {
                var insertStavkaResponse = _komercijalnoApiManager.PostAsync<StavkaCreateRequest, TD.Komercijalno.Contracts.Dtos.Stavke.StavkaDto>("/stavke",
                    new StavkaCreateRequest()
                    {
                        RobaId = stavka.RobaID,
                        VrDok = vrDok,
                        BrDok = insertDokumentResponse.Payload.BrDok,
                        Kolicina = stavka.Kolicina,
                        ProdajnaCenaBezPdv = stavka.VpCena
                    }).GetAwaiter().GetResult();

                if (insertStavkaResponse.NotOk)
                {
                    _logger.LogError($"Error inserting stavka komercijalno Api POST /stavke!");
                    return;
                }
            }

            var updatePorudzbinaResponse = _webshopApiManager.PostRawAsync($"/webshop/porudzbina/brdokkomercijalno/set?porudzbinaid={porudzbinaId}&brDokKomercijalno={insertDokumentResponse.Payload.BrDok}").GetAwaiter().GetResult();
            if (updatePorudzbinaResponse.NotOk)
            {
                _logger.LogError($"ERROR: (/webshop/porudzbina/brdokkomercijalno/set?porudzbinaid={porudzbinaId}&brDokKomercijalno={insertDokumentResponse.Payload.BrDok}) returned status [${updatePorudzbinaResponse.Status}]");
                return;
            }

            updatePorudzbinaResponse = _webshopApiManager.PostRawAsync($"/webshop/porudzbina/status/set?porudzbinaid={porudzbinaId}&status=1").GetAwaiter().GetResult();
            if (updatePorudzbinaResponse.NotOk)
            {
                _logger.LogError($"ERROR: (/webshop/porudzbina/status/set?porudzbinaid={{porudzbinaId}}&status=1) returned status [${updatePorudzbinaResponse.Status}]");
                return;
            }
        }

        public Task StartListeningWebshopAkcAsync()
        {
            _taskSchedulerManager.AddTask(new Core.Contracts.Tasks.Task(() =>
            {
                var resp = _webshopApiManager.GetRawAsync<List<AkcDto>>(WebApiEndpoints.GetAkcList()).GetAwaiter().GetResult();

                if (resp == null || resp.Payload == null || resp.Payload.IsEmpty())
                    return;

                foreach (var akc in resp.Payload)
                {
                    string[] parts = akc.Action.Split('|');
                    int porudzbinaId;
                    _logger.LogInformation("Start action: " + akc.Action);
                    switch (parts[0])
                    {
                        case "SENDSMS":
                            //_logger.LogError(Contracts.Messages.CommonMessages.ActionNotHandledMessage(parts[0]));
                            continue;
                        case "PretvoriUProracun":

                            if (!int.TryParse(parts[1], out porudzbinaId))
                                _logger.LogError($"Could not parse '{porudzbinaId}' into '{nameof(porudzbinaId)}'");

                            PretvoriUDokument(porudzbinaId, 32);
                            break;
                        case "PretvoriUPonudu":

                            if (!int.TryParse(parts[1], out porudzbinaId))
                                _logger.LogError($"Could not parse '{porudzbinaId}' into '{nameof(porudzbinaId)}'");

                            PretvoriUDokument(porudzbinaId, 34);
                            break;
                        default:
                            //_logger.LogError(Contracts.Messages.CommonMessages.ActionNotHandledMessage(parts[0]));
                            continue;
                    }
                    _logger.LogInformation("Removing action from web...");
                    var removeResp = _webshopApiManager.PostRawAsync($"/api/akc/delete/{akc.ID}").GetAwaiter().GetResult();
                    _logger.LogInformation("End action.");
                }
            }, TimeSpan.FromSeconds(1)));
            return _taskSchedulerManager.RunTasksAsync(true);
        }
    }
}
