using TD.Komercijalno.Contracts.Dtos.Mesto;

namespace TD.Komercijalno.Contracts.IManagers
{
	public interface IMestoManager
	{
		List<MestoDto> GetMultiple();
	}
}
