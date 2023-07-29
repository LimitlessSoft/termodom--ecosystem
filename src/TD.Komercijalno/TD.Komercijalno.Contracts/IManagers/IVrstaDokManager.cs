using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IVrstaDokManager
    {
        public ListResponse<VrstaDokDto> GetMultiple();
    }
}
