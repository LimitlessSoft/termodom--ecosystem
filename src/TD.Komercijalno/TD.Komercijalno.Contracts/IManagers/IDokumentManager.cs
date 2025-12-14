using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IDokumentManager
{
	DokumentDto Get(DokumentGetRequest request);
	List<DokumentDto> GetMultiple(DokumentGetMultipleRequest request);
	DokumentDto Create(DokumentCreateRequest request);
	string NextLinked(DokumentNextLinkedRequest request);
	void SetNacinPlacanja(DokumentSetNacinPlacanjaRequest request);
	void SetDokOut(DokumentSetDokOutRequest request);
	void SetFlag(DokumentSetFlagRequest request);
}
