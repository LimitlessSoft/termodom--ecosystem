using TD.Komercijalno.Contracts.Entities;

var bazaConnString = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
Console.WriteLine("===");
Console.WriteLine("Baza: " + bazaConnString);
Console.WriteLine("===");
Console.WriteLine(
    "Ova akcija ce proci kroz tabelu STAVKA i naci sve gde je TREN_STANJE < 0. Ispisace sve magacine gde je nasao ovakav slucaj."
);
Console.WriteLine("Ovo je brza akcija kojom ce pokazati da li postoji magacin sa neispravnom karticom.");
Console.WriteLine("Ovo nije najtacnija akcija jer ne sredjuje kartice vec samo uzima gotov rezultat!");
Console.WriteLine("Pritisni bilo koje dugme da nastavis...");
Console.Read();
Console.WriteLine("Zapocinjem...");
var source = new DB(
    $"data source=4monitor; initial catalog = {bazaConnString}; user=SYSDBA; password=m"
);
using var ctx = source.CreateContext();
var neispravniMagacini = ctx.Stavke.Where(x => x.TrenStanje < 0).Select(x => x.MagacinId).Distinct().ToList();
if(neispravniMagacini.Any())
{
    Console.WriteLine("Magacini sa neispravnim karticama: ");
    foreach(var mag in neispravniMagacini)
        Console.WriteLine(mag);
    Console.WriteLine("Gotovo!");
    return;
}
Console.WriteLine("Svi magacini su OK!");
Console.WriteLine("Gotovo!");
