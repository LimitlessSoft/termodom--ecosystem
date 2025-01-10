using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Parametri;

#if DEBUG
var environment = TDKomercijalnoEnvironment.Development;
#else
var environment = TDKomercijalnoEnvironment.Production;
#endif

Console.WriteLine("Starting Komercijalno danas update...");
Console.WriteLine($"Environment: {environment}");
Dictionary<int, TDKomercijalnoClient> clients = new();

var yearsInPast = 1;
Console.WriteLine("Initializing clients...");
Console.WriteLine("Years in past: " + yearsInPast);
for (var i = DateTime.UtcNow.Year; i >= DateTime.UtcNow.Year - yearsInPast; i--)
{
    clients.Add(i, new TDKomercijalnoClient(i, environment, TDKomercijalnoFirma.TCMDZ));
    clients.Add(i, new TDKomercijalnoClient(i, environment, TDKomercijalnoFirma.Termodom));
    clients.Add(i, new TDKomercijalnoClient(i, environment, TDKomercijalnoFirma.Magacin));
}

var currentTimeInBelgradeTimezone = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Central European Standard Time");
Console.WriteLine("Current time in Belgrade with daylight saving: " + currentTimeInBelgradeTimezone);

var updateParametriRequest = new UpdateParametarRequest
{
    Naziv = "danas",
    Vrednost = currentTimeInBelgradeTimezone.ToString("dd.MM.yyyy")
};

Console.WriteLine("Updating parametri...");
var tasks =
    clients.Select(client =>
        client.Value.Parametri.UpdateAsync(updateParametriRequest))
        .ToList();

Console.WriteLine("Waiting for all tasks to complete...");
await Task.WhenAll(tasks);
Console.WriteLine("All tasks completed.");
Console.WriteLine("Komercijalno danas update finished.");