using LSCore.SortAndPage.Contracts;
using TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;

public interface IInterneOtpremniceManager
{
	Task<InternaOtpremnicaDetailsDto> GetAsync(IdRequest request);
	InternaOtpremnicaDto Create(InterneOtpremniceCreateRequest request);
	Task<LSCoreSortedAndPagedResponse<InternaOtpremnicaDto>> GetMultipleAsync(
		GetMultipleRequest request
	);

	InternaOtpremnicaItemDto SaveItem(InterneOtpremniceItemCreateRequest request);
	void DeleteItem(InterneOtpremniceItemDeleteRequest request);
	void ChangeState(IdRequest request, InternaOtpremnicaStatus state);
	Task<InternaOtpremnicaDetailsDto> ForwardToKomercijalnoAsync(IdRequest request);
}
