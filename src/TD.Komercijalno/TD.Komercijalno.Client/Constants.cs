using LSCore.Common.Extensions;

namespace TD.Komercijalno.Client;

public static class Constants
{
	public static string KomercijalnoApiUrlFormat(
		int year,
		TDKomercijalnoEnvironment env,
		TDKomercijalnoFirma firma
	) => $"https://{year}{firma.GetDescription()}-komercijalno{env.GetDescription()}.termodom.rs";
}
