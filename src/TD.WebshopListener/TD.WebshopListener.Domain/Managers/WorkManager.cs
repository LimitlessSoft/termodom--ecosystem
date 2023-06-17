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


            // TODO: Ovo izmeniti, mapirati magacine na sajtu kako treba
            porudzbina.MagacinID += 100;
            var insertDokumentResponse = _tdBrainApiManager.PostAsync<TDBrainDokumentInsertRequest, int>("/komercijalno/dokument/insert", new TDBrainDokumentInsertRequest()
            {
                BazaId = porudzbina.MagacinID,
                DozvoliDaljeIzmeneUKomercijalnom = true,
                GodinaBaze = DateTime.Now.Year,
                InterniBroj = $"WEB {porudzbinaId}",
                KomercijalnoKorisnikId = 1,
                MagacinId = porudzbina.MagacinID,
                NuId = porudzbina.NacinPlacanja,
                VrDok = 32
            }).GetAwaiter().GetResult();

            // TODO: Implementirati document update preko TDBrain
            //dokument.AliasU = porudzbina.KorisnikID;
            //dokument.OpisUpl = porudzbina.ImeIPrezime;
            //dokument.Update(con);

            foreach(var stavka in stavke)
            {
                var inserttStavkaResponse = _tdBrainApiManager.PostAsync<TDBrainStavkaInsertRequeust>("/komercijalno/stavka/insert", new TDBrainStavkaInsertRequeust()
                {
                    RobaId = stavka.RobaID,
                    BazaId = porudzbina.MagacinID,
                    GodinaBaze = DateTime.Now.Year,
                    VrDok = 32,
                    BrDok = insertDokumentResponse.Payload,
                    Kolicina = stavka.Kolicina
                }).GetAwaiter().GetResult();
            }
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
