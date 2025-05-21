using TD.Office.Public.Contracts.Dtos.KomercijalnoMagacinFirma;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IKomercijalnoMagacinFirmaManager
{
	GetKomercijalnoMagacinFirmaDto Get(int magacinId);
}
