namespace TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Interfaces.IManagers;

public interface IKomercijalnoProveriCeneUMagacinimaManager
{
	Task GenerateReportAsync();
	string GetIzvestajNeispravnihCenaUMagacinimaStatus();
	DateTime? GetIzvjestajNeispravnihCenaUMagacinimaLastRun();
}
