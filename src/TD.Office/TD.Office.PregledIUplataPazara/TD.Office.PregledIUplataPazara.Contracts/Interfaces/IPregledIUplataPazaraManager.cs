using TD.Office.PregledIUplataPazara.Contracts.Requests;
using TD.Office.PregledIUplataPazara.Contracts.Responses;

namespace TD.Office.PregledIUplataPazara.Contracts.Interfaces;

public interface IPregledIUplataPazaraManager
{
	Task<PregledIUplataPazaraResponse> GetAsync(GetPregledIUplataPazaraRequest request);
	Task<PregledIUplataPazaraNeispravneStavkeIzvodaResponse> GetNeispravneStavkeIzvoda();
}
