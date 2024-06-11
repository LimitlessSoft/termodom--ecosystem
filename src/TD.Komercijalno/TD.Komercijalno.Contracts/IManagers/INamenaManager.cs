using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface INamenaManager
    {
        List<NamenaDto> GetMultiple();
    }
}
