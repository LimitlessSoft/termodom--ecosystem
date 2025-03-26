using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ISpecifikacijaNovcaManager
{
	/// <summary>
	/// Retrieves the money specification for the current day and the warehouse of the current user.
	/// If a specification does not exist, a new one is created.
	/// </summary>
	/// <returns></returns>
	Task<GetSpecifikacijaNovcaDto> GetCurrentAsync();
	Task<GetSpecifikacijaNovcaDto> GetSingleAsync(GetSingleSpecifikacijaNovcaRequest request);
	void Save(SaveSpecifikacijaNovcaRequest request);
	Task<GetSpecifikacijaNovcaDto> GetNextAsync(GetNextSpecifikacijaNovcaRequest request);
	Task<GetSpecifikacijaNovcaDto> GetPrevAsync(GetPrevSpecifikacijaNovcaRequest request);
	Task<GetSpecifikacijaNovcaDto> GetSpecifikacijaByDate(GetSpecifikacijaByDateRequest request);
}
