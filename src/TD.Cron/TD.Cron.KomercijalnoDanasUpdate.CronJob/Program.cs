using TD.Komercijalno.Client;
using TD.Komercijalno.Contracts.Requests.Parametri;

#if DEBUG
var environment = TDKomercijalnoEnvironment.Development;
#else
var environment = TDKomercijalnoEnvironment.Production;
#endif

Dictionary<int, TDKomercijalnoClient> clients = new();

var yearsInPast = 1;
for(var i = DateTime.UtcNow.Year; i >= DateTime.UtcNow.Year - yearsInPast; i--)
    clients.Add(i, new TDKomercijalnoClient(i, environment));

var currentTimeInBelgradeWithDaylightSaving = DateTime.UtcNow.AddHours(2);

var updateParametriRequest = new UpdateParametarRequest
{
    Naziv = "danas",
    Vrednost = currentTimeInBelgradeWithDaylightSaving.ToString("dd.MM.yyyy")
};

var tasks =
    clients.Select(client =>
        client.Value.Parametri.UpdateAsync(updateParametriRequest))
        .ToList();

await Task.WhenAll(tasks);