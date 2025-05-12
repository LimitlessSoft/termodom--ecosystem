using LSCore.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TD.Common.Vault.DependencyInjection;
using TD.Cron.KomercijalnoProveriCeneUMagacinima.CronJob;
using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Common.Contracts.Dtos;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Common.Repository;
using TD.Office.Common.Repository.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder
	.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
	.AddEnvironmentVariables()
	.AddVault<SecretsDto>();

builder.AddLSCoreLogging();
builder.Services.AddSingleton<IConfigurationRoot>(builder.Configuration);
builder.Services.RegisterDatabase();
builder.Services.AddSingleton<ISettingRepository, SettingRepository>();
var app = builder.Build();
var env = TDKomercijalnoEnvironment.Production;
var reperniMagacin = 150;
var danas = TimeZoneInfo.ConvertTime(
	DateTime.Now,
	TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
);
var proveriUOvimMagacinima = new List<Tuple<TDKomercijalnoFirma, int>>
{
	// new(TDKomercijalnoFirma.TCMDZ, 112),
	// new(TDKomercijalnoFirma.TCMDZ, 113),
	// new(TDKomercijalnoFirma.TCMDZ, 114),
	// new(TDKomercijalnoFirma.TCMDZ, 115),
	// new(TDKomercijalnoFirma.TCMDZ, 116),
	// new(TDKomercijalnoFirma.TCMDZ, 117),
	// new(TDKomercijalnoFirma.TCMDZ, 118),
	// new(TDKomercijalnoFirma.TCMDZ, 119),
	// new(TDKomercijalnoFirma.TCMDZ, 120),
	// new(TDKomercijalnoFirma.TCMDZ, 121),
	// new(TDKomercijalnoFirma.TCMDZ, 122),
	// new(TDKomercijalnoFirma.TCMDZ, 123),
	// new(TDKomercijalnoFirma.TCMDZ, 124),
	// new(TDKomercijalnoFirma.TCMDZ, 125),
	// new(TDKomercijalnoFirma.TCMDZ, 126),
	// new(TDKomercijalnoFirma.TCMDZ, 127),
	// new(TDKomercijalnoFirma.TCMDZ, 128),
	// new(TDKomercijalnoFirma.TCMDZ, 2112),
	// new(TDKomercijalnoFirma.TCMDZ, 2113),
	// new(TDKomercijalnoFirma.TCMDZ, 2114),
	// new(TDKomercijalnoFirma.TCMDZ, 2115),
	// new(TDKomercijalnoFirma.TCMDZ, 2116),
	// new(TDKomercijalnoFirma.TCMDZ, 2117),
	// new(TDKomercijalnoFirma.TCMDZ, 2118),
	// new(TDKomercijalnoFirma.TCMDZ, 2119),
	// new(TDKomercijalnoFirma.TCMDZ, 2120),
	// new(TDKomercijalnoFirma.TCMDZ, 2121),
	// new(TDKomercijalnoFirma.TCMDZ, 2122),
	// new(TDKomercijalnoFirma.TCMDZ, 2123),
	// new(TDKomercijalnoFirma.TCMDZ, 2124),
	// new(TDKomercijalnoFirma.TCMDZ, 2125),
	// new(TDKomercijalnoFirma.TCMDZ, 2126),
	// new(TDKomercijalnoFirma.TCMDZ, 2127),
	// new(TDKomercijalnoFirma.TCMDZ, 2128),
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
};
var clients = new Dictionary<TDKomercijalnoFirma, TDKomercijalnoClient>
{
	{
		TDKomercijalnoFirma.TCMDZ,
		new TDKomercijalnoClient(danas.Year, env, TDKomercijalnoFirma.TCMDZ)
	}
};
var reperneCeneDanasTask = clients[TDKomercijalnoFirma.TCMDZ]
	.Procedure.GetProdajnaCenaNaDanOptimizedAsync(
		new ProceduraGetProdajnaCenaNaDanOptimizedRequest
		{
			Datum = danas,
			MagacinId = reperniMagacin,
		}
	);
var report = new List<ReportItemDto>();
foreach (var t in proveriUOvimMagacinima)
{
	if (!clients.ContainsKey(t.Item1))
		clients.Add(t.Item1, new TDKomercijalnoClient(danas.Year, env, t.Item1));

	var ceneUMagacinu = clients[t.Item1]
		.Procedure.GetProdajnaCenaNaDanOptimizedAsync(
			new ProceduraGetProdajnaCenaNaDanOptimizedRequest { Datum = danas, MagacinId = t.Item2 }
		);

	var reperneCeneDanas = await reperneCeneDanasTask;
	var ceneUMagacinuDanas = await ceneUMagacinu;
	foreach (var c in reperneCeneDanas)
	{
		var cenaUMagacinu = ceneUMagacinuDanas.FirstOrDefault(x => x.RobaId == c.RobaId);
		if (cenaUMagacinu == null)
		{
			report.Add(
				new ReportItemDto()
				{
					MagacinId = t.Item2,
					RobaId = c.RobaId,
					Baza = t.Item1.ToString(),
					ProblemSaRobom = new ReportItemDto.ReportItemProblemSaRobomDto()
					{
						Opis = "Proizvod koji postoji u repernom magacinu ne postoji u trenutnom."
					}
				}
			);
			continue;
		}

		if (c.ProdajnaCenaBezPDV == cenaUMagacinu.ProdajnaCenaBezPDV)
			continue;

		report.Add(
			new ReportItemDto()
			{
				MagacinId = t.Item2,
				RobaId = c.RobaId,
				Baza = t.Item1.ToString(),
				ProblemSaCenom = new ReportItemDto.ReportItemProblemSaCenomDto()
				{
					TrenutnaCena = cenaUMagacinu.ProdajnaCenaBezPDV,
					CenaTrebaDaBude = c.ProdajnaCenaBezPDV
				}
			}
		);
	}
}
var text = JsonConvert.SerializeObject(report);
Console.WriteLine(text);
var settingRepository = app.Services.GetService<ISettingRepository>()!;
settingRepository.SetValue(
	SettingKey.PARTNERI_PO_GODINAMA_KOMERCIJALNO_FINANSIJSKO_PERIOD_GODINA,
	text
);
