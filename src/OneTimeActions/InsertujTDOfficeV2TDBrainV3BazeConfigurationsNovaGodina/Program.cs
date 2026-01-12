// Nakon konfigurisanja sa DEV strane, poslednji koraj jeste azuriranje (dodavanje)
// putanja baze za svaki magacin za novu godinu.
// Oni se generalno malo razlikuju, ali odredjeni magacini gadjaju druge baze.
// Ovo je skladisteno u TDOffice_v3 i uz pomoc API-a mozemo dodati nove baze.

using System.Net.Http.Json;

var client = new HttpClient()
{
    BaseAddress = new Uri("http://localhost:7207")
};

var year = DateTime.Now.Year;
var bazaTCMD = $"c:/poslovanje/baze/{year}/FRANSIZA{year}TCMD.fdb";
var bazaTD = $"c:/poslovanje/baze/{year}/TERMODOM{year}.fdb";
var sasa = $"c:/poslovanje/baze/{year}/SASA{year} SASA PDV.fdb"; // Pojma nemam sta ce mi ova, ali znam da nesto radimo nad svim bazama pa mi treba ovde
var magacin = $"c:/poslovanje/baze/{year}/FRANSIZA{year} MAGACIN.fdb"; // Pojma nemam sta ce mi ova, ali znam da nesto radimo nad svim bazama pa mi treba ovde
var vhemza = $"C:/Poslovanje/Baze/Vhemza/{year}/VHEMZA{year}.FDB"; // Pojma nemam sta ce mi ova, ali znam da nesto radimo nad svim bazama pa mi treba ovde

var magaciniBaza = new Dictionary<int, string>()
{
    { 112, bazaTCMD },
    { 113, bazaTCMD },
    { 114, bazaTCMD },
    { 115, bazaTCMD },
    { 116, bazaTCMD },
    { 117, bazaTCMD },
    { 118, bazaTCMD },
    { 119, bazaTCMD },
    { 120, bazaTCMD },
    { 121, bazaTCMD },
    { 122, bazaTCMD },
    { 123, vhemza },
    { 124, bazaTCMD },
    { 125, bazaTCMD },
    { 126, bazaTCMD },
    { 127, bazaTCMD },
    { 128, bazaTCMD },
    { 250, vhemza },
    { 40, magacin },
    { 150, vhemza },
    { 170, bazaTCMD },
    { 12, bazaTD },
    { 13, bazaTD },
    { 14, bazaTD },
    { 15, bazaTD },
    { 16, bazaTD },
    { 17, bazaTD },
    { 18, bazaTD },
    { 19, bazaTD },
    { 20, bazaTD },
    { 21, bazaTD },
    { 22, bazaTD },
    { 23, bazaTD },
    { 24, bazaTD },
    { 25, bazaTD },
    { 26, bazaTD },
    { 27, bazaTD },
    { 28, bazaTD },
    { 50, bazaTD },
    { 997, sasa }
};

var ok = ValidirajDaSveBazePostoje();
if(ok)
    await InsertujAsync();

Console.WriteLine("Pristisni bilo koje dugme da izadjes...");
Console.ReadLine();

return;

async Task InsertujAsync()
{
    Console.WriteLine("Insertujem...");
    foreach(var k in magaciniBaza.Keys)
    {
        var baza = magaciniBaza[k];
        Console.WriteLine($"Insertujem {k} - {baza}...");
        // Create multipart form data content
        var content = new MultipartFormDataContent
        {
            { new StringContent(k.ToString()), "magacinId" },
            { new StringContent(year.ToString()), "godina" },
            { new StringContent(baza), "putanja" }
        };

        // Send the POST request
        var resp = await client.PostAsync("/dbsettings/baza/komercijalno/addorupdate", content);

        if (resp.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine($"{k} - {baza} insertovano.");
            continue;
        }

        Console.WriteLine($"Greska prilikom insertovanja: {resp.StatusCode}");
    }
    Console.WriteLine("Gotovo insertovanje svega!");
}
bool ValidirajDaSveBazePostoje()
{
    var allOk = true;
    Console.WriteLine("Proveravam da li svi fajlovi baza postoje...");
    var files = magaciniBaza.Values.Distinct();
    foreach(var file in files)
    {
        if (File.Exists(file))
        {
            Console.WriteLine($"[{file}] OK");
            continue;
        }
        Console.WriteLine($"GRESKA: {file} baze ne postoji!");
        allOk = false;
    }
    return allOk;
}