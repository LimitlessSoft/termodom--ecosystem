using ExcelDataReader;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.Dtos.MCNabavkaRobe;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.MCNabavkaRobe;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikKatBrRobaIds;
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

        public async Task<ListResponse<CenovnikItem>> UvuciFajlAsync(MCNabavkaRobeUvuciFajlRequest request)
        {
            var response = new ListResponse<CenovnikItem>();
            response.Payload = new List<CenovnikItem>();

            var mcPartnerCenovnikKatBrRobaIdsResponse = await _tdofficeApiManager.GetAsync<MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest, List<MCPartnerCenovnikKatBrRobaIdGetDto>>("/mc-partner-cenovnik-kat-br-roba-ids", new MCPartnerCenovnikKatBrRobaIdsGetMultipleRequest()
            {
                DobavljacPPID = request.DobavljacPPID
            });
            response.Merge(mcPartnerCenovnikKatBrRobaIdsResponse);
            if(response.NotOk)
                return response;

            var robaResponse = await _komercijalnoApiManager.GetAsync<RobaGetMultipleRequest, List<RobaDto>>("/roba", new RobaGetMultipleRequest()
            {
                Vrsta = 1
            });
            response.Merge(robaResponse);
            if (response.NotOk)
                return response;

            using(var stream = request.File.OpenReadStream())
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while(reader.Read())
                    {
                        string nazivArtiklaPro = reader.GetString(request.KolonaNaziv);

                        if (string.IsNullOrWhiteSpace(nazivArtiklaPro) || nazivArtiklaPro.ToLower().Contains("naziv"))
                            continue;

                        string jedinicaMerePro = reader.GetString(request.KolonaJediniceMere);
                        string katBrPro = reader.GetString(request.KolonaKataloskiBroj);

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
                            VezaId = veza?.Id
                        });
                    }
                }
            }

            return response;
        }
    }
}
