using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Contracts.IManagers;

public interface IDokumentTagIzvodManager
{
	List<DokumentTagIzvodGetDto> GetMultiple(DokumentTagIzvodGetMultipleRequest request);
	DokumentTagIzvodGetDto Save(DokumentTagizvodPutRequest request);
}
