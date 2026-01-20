namespace TD.Office.Common.Contracts
{
	public static class LegacyConstants
	{
		public const string ProjectName = "TD.Office";

		public static class PermissionGroup
		{
			public const string NavBar = "nav-bar";
			public const string Dashboard = "dashboard";
			public const string NalogZaPrevoz = "nalog-za-prevoz";
			public const string Korisnici = "korisnici";
			public const string KorisniciList = "korisnici-lista";
			public const string Web = "web";
			public const string Partneri = "partneri";
			public const string SpecifikacijaNovca = "specifikacija-novca";
			public const string PartnerIzvestajFinansijskoKomercijalno =
				"partneri-izvestaj-finansijko-komercijalno";
			public const string IzvestajUkupneKolicinePoRobiUFiltriranimDokumentima =
				"izvestaj-ukupne-kolicine-po-robi-u-filtriranim-dokumentima";
			public const string Proracuni = "proracuni";
			public const string IzvestajIzlazaRobePoGodinama = "izvestaj-izlaza-robe-po-godinama";
			public const string PartnerAnaliza = "partneri-analiza";
			public const string Otpremnice = "otpremnice";
			public const string IzvestajNeispravnihCenaUMagacinima =
				"izvestaj-neispravnih-cena-u-magacinima";
			public const string RobaPopis = "roba-popis";
			public const string MasovniSMS = "masovni-sms";
			public const string KalendarAktivnosti = "kalendar-aktivnosti";
			public const string TipOdsustva = "tip-odsustva";
			public const string TipKorisnika = "tip-korisnika";
			public const string ModuleHelp = "module-help";
		}

		public static class Jwt
		{
			public const string ConfigurationKey = "JWT_KEY";
			public const string ConfigurationIssuer = "JWT_ISSUER";
			public const string ConfigurationAudience = "JWT_AUDIENCE";
		}

		public static class DbMigrations
		{
			public static readonly string DbSeedsRoot = Path.Combine(
				Environment.CurrentDirectory,
				"DbSeeds"
			);
			public static readonly string DbSeedsDownRoot = Path.Combine(
				Environment.CurrentDirectory,
				"DbSeeds",
				"Down"
			);
		}
	}
}
