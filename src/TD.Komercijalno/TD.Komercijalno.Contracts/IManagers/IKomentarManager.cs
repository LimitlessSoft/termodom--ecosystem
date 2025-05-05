using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Contracts.IManagers
{
	public interface IKomentarManager
	{
		KomentarDto Get(GetKomentarRequest request);
		KomentarDto Create(CreateKomentarRequest request);
		KomentarDto Update(UpdateKomentarRequest request);
		void FlushComments(FlushCommentsRequest request);
	}
}
