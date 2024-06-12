using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IMagacinManager
    {
        List<MagacinDto> GetMultiple();
    }
}
