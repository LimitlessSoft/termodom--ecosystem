using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Parametri;

#if DEBUG
var environment = TDKomercijalnoEnvironment.Development;
#else
var environment = TDKomercijalnoEnvironment.Production;
#endif

Console.WriteLine("Starting Komercijalno danas update...");
Console.WriteLine($"Environment: {environment}");
HashSet<TDKomercijalnoClient> clients = new();

var yearsInPast = 1;
Console.WriteLine("Initializing clients...");
Console.WriteLine("Years in past: " + yearsInPast);
for (var i = DateTime.UtcNow.Year; i >= DateTime.UtcNow.Year - yearsInPast; i--)
{
	AddClient(i, TDKomercijalnoFirma.TCMDZ);
	AddClient(i, TDKomercijalnoFirma.Termodom);
	AddClient(i, TDKomercijalnoFirma.Magacin);
	AddClient(i, TDKomercijalnoFirma.Vhemza);
	AddClient(i, TDKomercijalnoFirma.SasaPdv);
}

var currentTimeInBelgradeTimezone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
	DateTime.UtcNow,
	"Central European Standard Time"
);
Console.WriteLine(
	"Current time in Belgrade with daylight saving: " + currentTimeInBelgradeTimezone
);

var updateParametriRequest = new UpdateParametarRequest
{
	Naziv = "danas",
	Vrednost = currentTimeInBelgradeTimezone.ToString("dd.MM.yyyy")
};

Console.WriteLine("Updating parametri...");
var tasks = clients.Select(client => client.Parametri.UpdateAsync(updateParametriRequest)).ToList();

Console.WriteLine("Waiting for all tasks to complete...");
await Task.WhenAll(tasks);
Console.WriteLine("All tasks completed.");
Console.WriteLine("Komercijalno danas update finished.");

return;

void AddClient(int year, TDKomercijalnoFirma firma)
{
	try
	{
		clients.Add(new TDKomercijalnoClient(year, environment, firma));
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Failed to add client for year {year} and firma {firma}: {ex.Message}");
	}
}
	