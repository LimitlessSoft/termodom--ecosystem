using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface INamenaManager
    {
        public ListResponse<NamenaDto> GetMultiple();
    }
}
