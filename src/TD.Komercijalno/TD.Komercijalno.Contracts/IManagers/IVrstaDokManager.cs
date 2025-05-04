using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IVrstaDokManager
{
	List<VrstaDokDto> GetMultiple();
}
