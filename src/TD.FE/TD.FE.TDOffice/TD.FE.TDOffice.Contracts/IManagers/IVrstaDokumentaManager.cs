using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.VrsteDokumenata;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IVrstaDokumentaManager
    {
        ListResponse<VrstaDokumentaDto> GetMultiple();
    }
}
