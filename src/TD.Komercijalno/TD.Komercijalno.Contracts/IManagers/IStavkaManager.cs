using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Contracts.IManagers
{
	public interface IStavkaManager
	{
		List<StavkaDto> GetMultiple(StavkaGetMultipleRequest request);
		StavkaDto Create(StavkaCreateRequest request);
		void DeleteStavke(StavkeDeleteRequest request);
	}
}
