using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface INacinPlacanjaManager
    {
        List<NacinPlacanjaDto> GetMultiple();
    }
}
