
using KopirajPartnereIzBazeUBazu;
using TD.Komercijalno.Domain.Managers;

PartnerManager.IGNORE_VALIDATION = true;
var izBazeConnectionString = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
var uBazuConnectionString = new List<string> {
    "C:\\Poslovanje\\Baze\\2026\\FRANSIZA2026TCMD.FDB",
    "C:\\Poslovanje\\Baze\\2026\\FRANSIZA2026 MAGACIN.FDB",
    "C:\\Poslovanje\\Baze\\2026\\SASA2026 SASA PDV.FDB",
    "C:\\Poslovanje\\Baze\\2026\\TERMODOM2026.FDB",
    "C:\\Poslovanje\\Baze\\VHEMZA\\2026\\VHEMZA2026.FDB",
};
Console.WriteLine("===");
Console.WriteLine("Iz baze: " + izBazeConnectionString);
Console.WriteLine("===");
Console.WriteLine(
    "Ova akcija ce kopirati partnere koji ne postoje U bazi. Poredi ih po PPID."
);
Console.WriteLine("Pritisni bilo koje dugme da nastavis.");
Console.ReadLine();

Console.WriteLine("Zapocinjem...");
var source = new DB(
    $"data source=4monitor; initial catalog = {izBazeConnectionString}; user=SYSDBA; password=m"
);
var sourceCtx = source.CreateContext();
Console.WriteLine("Ucitavam partnere IZ baze...");
var sourcePartners = sourceCtx.Partneri.ToList();

void Log(string dest, string msg)
{
    Console.WriteLine($"[{dest}] {msg}");
}

foreach (var dest in uBazuConnectionString)
{
    var destination = new DB(
        $"data source=4monitor; initial catalog = {dest}; user=SYSDBA; password=m"
    );
    using var destinationCtx = destination.CreateContext();
    // ot
    //Log(dest, "One time cleanup...");
    //var ot = destinationCtx.Partneri.Where(x => x.Ppid > 28913 && x.Ppid < 120654).ToList();
    //destinationCtx.Partneri.RemoveRange(ot);
    //destinationCtx.SaveChanges();
    // ot end
    Log(dest, "Ucitavam partnere U baze...");
    var destinationPartners = destinationCtx.Partneri.ToList();
    Log(dest, "Maksimalni ID (bez onih > 200_000): " + destinationPartners.Where(x => x.Ppid < 200_000).Max(x => x.Ppid));
    Log(dest, "Optimizuejem...");
    var destinationPartnersIds = destinationPartners.Select(x => x.Ppid).ToHashSet();
    Log(dest, "Filtriram nepostojece partnere U...");
    var partneriToAdd = sourcePartners.ToList();
    partneriToAdd.RemoveAll(x => destinationPartnersIds.Contains(x.Ppid));
    foreach (var partnerToAdd in partneriToAdd)
    {
        //Log(dest, "One time cleanup...");
        //var aa = destinationPartners.RemoveAll(x => x.Pib == partnerToAdd.Pib);
        destinationCtx.Partneri.Add(partnerToAdd);
        Log(dest, partnerToAdd.Naziv + " inserted");
    }
    if (partneriToAdd.Count == 0)
    {
        Console.WriteLine("Vec sinhronizovano.");
        continue;
    }
    Log(dest, "Saving changes...");
    destinationCtx.SaveChanges();
}
Console.WriteLine("Gotovo!");
Console.ReadLine();
