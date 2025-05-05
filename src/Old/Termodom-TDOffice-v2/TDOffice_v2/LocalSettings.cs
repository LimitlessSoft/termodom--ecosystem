using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace TDOffice_v2
{
	public class LocalSettings
	{
		public static LocalSettings Settings { get; set; }
		public static string appDataFolderPath { get; set; }

		public string tdofficeConnectionString { get; set; } =
			"data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\TDOffice_v2\\TDOffice_v2_2021.FDB; user=SYSDBA; password=m";

		public string lastUsername { get; set; }

		public bool TDPopis_PrilikomUnosaStavkeUnosimNarucenuKolicinu { get; set; } = false;
		public bool TDPopis_PrikaziDetaljnuAnalizu { get; set; } = true;
		public int indexPoslednjePrikazaneBeleske { get; set; } = 0;
		public bool KarticaRobe_RacunajVazecuNabavnuCenu { get; set; } = false;
		public bool KarticaRobe_BoldujDokument36 { get; set; } = false;
		public DateTime LastPatchLogSeen { get; set; } = DateTime.Now.AddYears(-1);
		public double DefinisanjeProdajneCene_MaximalniRabat { get; set; } = 100;

		static LocalSettings()
		{
			appDataFolderPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
				"TDOffice_v2"
			);
			Directory.CreateDirectory(appDataFolderPath);

			if (File.Exists(Path.Combine(appDataFolderPath, "Settings.td")))
			{
				Settings = JsonConvert.DeserializeObject<LocalSettings>(
					File.ReadAllText(Path.Combine(appDataFolderPath, "Settings.td"))
				);
			}
			else
			{
				Settings = new LocalSettings();
				Update();
			}
		}

		public static void Update()
		{
			File.WriteAllText(
				Path.Combine(appDataFolderPath, "Settings.td"),
				JsonConvert.SerializeObject(Settings)
			);
		}
	}
}
