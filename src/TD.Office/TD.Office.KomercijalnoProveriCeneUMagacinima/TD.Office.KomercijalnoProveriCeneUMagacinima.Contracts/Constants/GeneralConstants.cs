using TD.Komercijalno.Client;

namespace TD.Office.KomercijalnoProveriCeneUMagacinima.Contracts.Constants;

public static class GeneralConstants
{
	public static int ReperniMagacin = 150;
	public static DateTime Danas = TimeZoneInfo.ConvertTime(
		DateTime.Now,
		TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
	);
	public static List<Tuple<TDKomercijalnoFirma, int>> ProveriUOvimMagacinima =
	[
		new(TDKomercijalnoFirma.TCMDZ, 112),
		new(TDKomercijalnoFirma.TCMDZ, 113),
		// new(TDKomercijalnoFirma.TCMDZ, 114),
		// new(TDKomercijalnoFirma.TCMDZ, 115),
		new(TDKomercijalnoFirma.TCMDZ, 116),
		new(TDKomercijalnoFirma.TCMDZ, 117),
		new(TDKomercijalnoFirma.TCMDZ, 118),
		new(TDKomercijalnoFirma.TCMDZ, 119),
		new(TDKomercijalnoFirma.TCMDZ, 120),
		new(TDKomercijalnoFirma.TCMDZ, 121),
		new(TDKomercijalnoFirma.TCMDZ, 122),
		new(TDKomercijalnoFirma.TCMDZ, 123),
		// new(TDKomercijalnoFirma.TCMDZ, 124),
		new(TDKomercijalnoFirma.TCMDZ, 125),
		new(TDKomercijalnoFirma.TCMDZ, 126),
		new(TDKomercijalnoFirma.TCMDZ, 127),
		new(TDKomercijalnoFirma.TCMDZ, 128),
		new(TDKomercijalnoFirma.TCMDZ, 151),
		new(TDKomercijalnoFirma.TCMDZ, 152),
		new(TDKomercijalnoFirma.TCMDZ, 340),
		new(TDKomercijalnoFirma.TCMDZ, 2112),
		new(TDKomercijalnoFirma.TCMDZ, 2113),
		new(TDKomercijalnoFirma.TCMDZ, 2114),
		new(TDKomercijalnoFirma.TCMDZ, 2115),
		new(TDKomercijalnoFirma.TCMDZ, 2116),
		new(TDKomercijalnoFirma.TCMDZ, 2117),
		new(TDKomercijalnoFirma.TCMDZ, 2118),
		new(TDKomercijalnoFirma.TCMDZ, 2119),
		new(TDKomercijalnoFirma.TCMDZ, 2120),
		new(TDKomercijalnoFirma.TCMDZ, 2121),
		new(TDKomercijalnoFirma.TCMDZ, 2122),
		new(TDKomercijalnoFirma.TCMDZ, 2123),
		new(TDKomercijalnoFirma.TCMDZ, 2124),
		new(TDKomercijalnoFirma.TCMDZ, 2125),
		new(TDKomercijalnoFirma.TCMDZ, 2126),
		new(TDKomercijalnoFirma.TCMDZ, 2127),
		new(TDKomercijalnoFirma.TCMDZ, 2128),
		// new(TDKomercijalnoFirma.Vhemza, 213),
		// new(TDKomercijalnoFirma.Vhemza, 214),
		// new(TDKomercijalnoFirma.Vhemza, 215),
		// new(TDKomercijalnoFirma.Vhemza, 216),
		// new(TDKomercijalnoFirma.Vhemza, 217),
		// new(TDKomercijalnoFirma.Vhemza, 218),
		// new(TDKomercijalnoFirma.Vhemza, 219),
		// new(TDKomercijalnoFirma.Vhemza, 220),
		// new(TDKomercijalnoFirma.Vhemza, 220),
		// new(TDKomercijalnoFirma.Vhemza, 221),
		// new(TDKomercijalnoFirma.Vhemza, 222),
		new(TDKomercijalnoFirma.Vhemza, 223),
		// new(TDKomercijalnoFirma.Vhemza, 224),
		// new(TDKomercijalnoFirma.Vhemza, 225),
		// new(TDKomercijalnoFirma.Vhemza, 226),
		// new(TDKomercijalnoFirma.Vhemza, 227),
		// new(TDKomercijalnoFirma.Vhemza, 228),
		new(TDKomercijalnoFirma.Vhemza, 250),
		// new(TDKomercijalnoFirma.Vhemza, 252),
		new(TDKomercijalnoFirma.Vhemza, 2223),
	];
}
