using ExcelDataReader;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.Dtos.MCNabavkaRobe;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.MCNabavkaRobe;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikKatBrRobaIds;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Requests.MCPartnerCenovnikKatBrRobaId;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class MCNabavkaRobeManager : BaseManager<MCNabavkaRobeManager>, IMCNabavkaRobeManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        private readonly ITDOfficeApiManager _tdofficeApiManager;
        public MCNabavkaRobeManager(ILogger<MCNabavkaRobeManager> logger, IKomercijalnoApiManager komercijalnoApiManager, ITDOfficeApiManager tdOfficeApiManager)
            : base(logger)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _komercijalnoApiManager = komercijalnoApiManager;
            _tdofficeApiManager = tdOfficeApiManager;
        }

        public async Task<Response<bool>> ProveriPostojanjeCenovnikaNaDan(MCNabavkaRobeProveriPostojanjeCenovnikaNaDanRequest request)
        {
            var response = new Response<bool>()
            {
                Payload = false
            };

            var cenovnikItemsResponse = await _tdofficeApiManager.GetAsync<MCPartnerCenovnikItemGetRequest, List<MCpartnerCenovnikItemEntityGetDto>>("/mc-partner-cenovnik-items", new MCPartnerCenovnikItemGetRequest()
            {
                PPID = request.PPID,
                VaziOdDana = request.Datum
            });
            response.Merge(cenovnikItemsResponse);
            if (response.NotOk)
                return response;

            response.Payload = cenovnikItemsResponse.Payload.Count > 0;

            return response;
        }

        public async Task<ListResponse<MCNabavkaRobeUporediCenovnikeItemDto>> UporediCenovnikeAsync()
        {
            var response = new ListResponse<MCNabavkaRobeUporediCenovnikeItemDto>();
            response.Payload = new List<MCNabavkaRobeUporediCenovnikeItemDto>();

            var vezeResponse = await _tdofficeApiManager.GetAsync<MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest, List<MCPartnerCenovnikKatBrRobaIdGetDto>>("/mc-partner-cenovnik-kat-br-roba-ids", new MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest());
            response.Merge(vezeResponse);
            if(response.NotOk)
                return response;

            var cenovnikItemsResponse = await _tdofficeApiManager.GetAsync<MCPartnerCenovnikItemGetRequest, List<MCpartnerCenovnikItemEntityGetDto>>("/mc-partner-cenovnik-items", new MCPartnerCenovnikItemGetRequest());
            response.Merge(cenovnikItemsResponse);
            if (response.NotOk)
                return response;

            var robaResponse = await _komercijalnoApiManager.GetAsync<RobaGetMultipleRequest, List<RobaDto>>("/roba", new RobaGetMultipleRequest()
            {
                Vrsta = 1
            });
            response.Merge(robaResponse);
            if (response.NotOk)
                return response;

            var dict = new Dictionary<int, MCNabavkaRobeUporediCenovnikeItemDto>();
            foreach (var robaVeza in vezeResponse.Payload)
                if(!dict.ContainsKey(robaVeza.RobaId))
                    dict.Add(robaVeza.RobaId, new MCNabavkaRobeUporediCenovnikeItemDto()
                    {
                        RobaId = robaVeza.RobaId
                    });

            var ppids = cenovnikItemsResponse.Payload.Select(x => x.PPID).Distinct();

            foreach(var ppid in ppids)
            {
                var filtered = cenovnikItemsResponse.Payload.Where(x => x.PPID == ppid);
                var maxDate = filtered.Max(x => x.VaziOdDana);

                var items = filtered.Where(x => x.VaziOdDana == maxDate);

                foreach (var item in items)
                {
                    var veza = vezeResponse.Payload.FirstOrDefault(x => x.DobavljacPPID == ppid && x.KatBrProizvodjaca == item.KatBr);
                    if (veza == null)
                        continue;

                    var dtoRef = dict[veza.RobaId];
                    dtoRef.SubItems.Add(new MCNabavkaRobeUporediCenovnikeSubItemDto()
                    {
                        DobavljacPPID = ppid,
                        VPCenaSaPopustom = item.VpCenaBezRabata * ((100 - item.Rabat) / 100),
                        DobavljacKatBr = item.KatBr
                    });
                }
            }

            foreach (var val in dict.Values)
            {
                var r = robaResponse.Payload.FirstOrDefault(x => x.RobaId == val.RobaId);
                val.KatBr = r == null ? "Undefined" : r.KatBr;
                val.Naziv = r == null ? "Undefined" : r.Naziv;
                val.JM = r == null ? "Undefined" : r.JM;
                response.Payload.Add(val);
            }

            return response;
        }

        public async Task<ListResponse<CenovnikItem>> UvuciFajlAsync(MCNabavkaRobeUvuciFajlRequest request)
        {
            var response = new ListResponse<CenovnikItem>();
            response.Payload = new List<CenovnikItem>();

            var mcPartnerCenovnikKatBrRobaIdsResponse = await _tdofficeApiManager.GetAsync<MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest, List<MCPartnerCenovnikKatBrRobaIdGetDto>>("/mc-partner-cenovnik-kat-br-roba-ids", new MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest()
            {
                DobavljacPPID = request.DobavljacPPID
            });
            response.Merge(mcPartnerCenovnikKatBrRobaIdsResponse);
            if (response.NotOk)
                return response;

            Task<Response<List<MCpartnerCenovnikItemEntityGetDto>>> mcPartnerCenovnikItemsResponse = null;
            if (request.SacuvajUBazu)
            {
                request.VaziOdDana = DateTime.SpecifyKind(request.VaziOdDana.Value, DateTimeKind.Utc);
                mcPartnerCenovnikItemsResponse = _tdofficeApiManager.GetAsync<MCPartnerCenovnikItemGetRequest, List<MCpartnerCenovnikItemEntityGetDto>>("/mc-partner-cenovnik-items", new MCPartnerCenovnikItemGetRequest()
                {
                    PPID = request.DobavljacPPID,
                    VaziOdDana = request.VaziOdDana.Value
                });
            }
            var robaResponse = await _komercijalnoApiManager.GetAsync<RobaGetMultipleRequest, List<RobaDto>>("/roba", new RobaGetMultipleRequest()
            {
                Vrsta = 1
            });
            response.Merge(robaResponse);
            if (response.NotOk)
                return response;

            if (request.SacuvajUBazu)
            {
                response.Merge(await mcPartnerCenovnikItemsResponse);
                if(response.NotOk)
                    return response;

                foreach (var item in (await mcPartnerCenovnikItemsResponse).Payload)
                {
                    response.Merge(await _tdofficeApiManager.DeleteAsync($"/mc-partner-cenovnik-items/{item.Id}"));
                    if(response.NotOk)
                        return response;
                }

            }

            using (var stream = request.File.OpenReadStream())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        try
                        {
                            string nazivArtiklaPro = reader.GetString(request.KolonaNaziv);

                            if (string.IsNullOrWhiteSpace(nazivArtiklaPro) || nazivArtiklaPro.ToLower().Contains("naziv"))
                                continue;

                            var jedinicaMerePro = reader.GetString(request.KolonaJediniceMere);
                            var katBrPro = reader.GetString(request.KolonaKataloskiBroj);
                            var vpCenaBezRabata = reader.GetDouble(request.KolonaVPCenaBezRabata);
                            var rabat = request.KolonaRabat == null ? 0 : reader.GetDouble(request.KolonaRabat.Value);
                            var vpCenaSaRabatom = vpCenaBezRabata * ((100d - rabat) / 100);
                            var veza = mcPartnerCenovnikKatBrRobaIdsResponse.Payload?.FirstOrDefault(x => x.KatBrProizvodjaca == katBrPro);
                            var robaKomercijalno = robaResponse.Payload?.FirstOrDefault(x => x.RobaId == veza?.RobaId);

                            response.Payload.Add(new CenovnikItem()
                            {
                                KatBrPro = katBrPro,
                                JMPro = jedinicaMerePro,
                                NazivPro = nazivArtiklaPro,
                                FoundInRoba = robaKomercijalno != null,
                                JM = robaKomercijalno?.JM,
                                KatBr = robaKomercijalno?.KatBr,
                                Naziv = robaKomercijalno?.Naziv,
                                VPCenaSaRabatom = vpCenaSaRabatom,
                                VezaId = veza?.Id,
                                KomercijalnoRobaId = robaKomercijalno?.RobaId
                            });

                            if(request.SacuvajUBazu)
                            {
                                response.Merge(await _tdofficeApiManager.PutAsync<SaveMCPartnerCenovnikItemRequest, MCPartnerCenovnikItemEntity>("/mc-partner-cenovnik-items", new SaveMCPartnerCenovnikItemRequest()
                                {
                                    KatBr = katBrPro,
                                    PPID = request.DobavljacPPID,
                                    Rabat = rabat,
                                    VaziOdDana = request.VaziOdDana.Value,
                                    VpCenaBezRabata = vpCenaBezRabata
                                }));
                                if (response.NotOk)
                                    return response;
                            }
                        }
                        catch(Exception ex)
                        {
                            var a = ex.ToString();
                        }
                    }
                }
            }

            return response;
        }
    }
}
