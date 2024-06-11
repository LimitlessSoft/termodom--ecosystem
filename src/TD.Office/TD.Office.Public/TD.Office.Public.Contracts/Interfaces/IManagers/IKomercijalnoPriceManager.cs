using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IKomercijalnoPriceManager
    {
        List<KomercijalnoPriceGetDto> GetMultiple();
    }
}
