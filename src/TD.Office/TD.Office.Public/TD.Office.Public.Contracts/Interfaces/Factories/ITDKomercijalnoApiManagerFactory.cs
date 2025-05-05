using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Contracts.Interfaces.Factories;

public interface ITDKomercijalnoApiManagerFactory
{
	ITDKomercijalnoApiManager Create(int year);
	Dictionary<int, ITDKomercijalnoApiManager> Create(List<int> year);
}
