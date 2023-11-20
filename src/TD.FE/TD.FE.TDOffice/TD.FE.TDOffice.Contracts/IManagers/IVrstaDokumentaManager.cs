using LSCore.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.VrsteDokumenata;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IVrstaDokumentaManager
    {
        LSCoreListResponse<VrstaDokumentaDto> GetMultiple();
    }
}
