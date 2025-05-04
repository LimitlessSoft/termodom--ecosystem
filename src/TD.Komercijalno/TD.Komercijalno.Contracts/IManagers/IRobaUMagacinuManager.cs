using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;

namespace TD.Komercijalno.Contracts.IManagers
{
	public interface IRobaUMagacinuManager
	{
		List<RobaUMagacinuGetDto> GetMultiple(RobaUMagacinuGetMultipleRequest request);
	}
}
