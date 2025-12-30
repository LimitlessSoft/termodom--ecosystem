
using TD.Komercijalno.Contracts.Entities;

var bazaConnString = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
Console.WriteLine("===");
Console.WriteLine("Baza: " + bazaConnString);
Console.WriteLine("===");
Console.WriteLine("Ova akcija ce azurirati nabavne cene u pocetnom stanju gde god je nabavna cena == 0");
Console.WriteLine("Nabavnu cenu koju ce uzeti je ona iz RobaUMagacinu za dati magacin.");
var source = new DB($"data source=4monitor; initial catalog = {bazaConnString}; user=SYSDBA; password=m");
var ctx = source.CreateContext();
int vrDok = 0;
int brDok = 0;
Dokument? dok;
void UnosBrojaDokumenta()
{
    Console.WriteLine();
    Console.Write("Unesite broj dokumenta pocetnog stanja: ");
    var b = Console.ReadLine();
    if (!int.TryParse(b, out brDok))
    {
        Console.WriteLine("Neispravan broj dokumenta!");
        UnosBrojaDokumenta();
    }
    dok = ctx.Dokumenti.FirstOrDefault(x => x.VrDok == vrDok && x.BrDok == brDok);
    if(dok == null)
    {
        Console.WriteLine("Dokument nije pronadjen!");
        UnosBrojaDokumenta();
    }
}
UnosBrojaDokumenta();

var sifarnik = ctx.Roba.ToDictionary(x => x.Id, x => x);
var robaUMagacinuSve = ctx.RobaUMagacinu.Where(x => x.MagacinId == dok!.MagacinId).ToDictionary(x => x.RobaId, x => x);
void PrintHeader(int robaId)
{
    Console.WriteLine();
    var roba = sifarnik[robaId];
    Console.WriteLine("===");
    Console.WriteLine($"[{roba.KatBr}] {roba.Naziv}");
}
void PrintFooter()
{
    Console.WriteLine("===");
}

var stavke = ctx.Stavke.Where(x => x.VrDok == vrDok && x.BrDok == brDok).ToList();
foreach(var stavka in stavke)
{
    if(stavka.NabavnaCena == 0)
    {
        PrintHeader(stavka.RobaId);
        Console.WriteLine($"Nabavna cena 0. Azuriram...");
        var rum = robaUMagacinuSve[stavka.RobaId];
        var nabavnaCena = rum.NabavnaCena;
        if(nabavnaCena == 0)
        {
            Console.WriteLine("Nabavna cena koju sam pronasao u RobaUMagacinu je takodje 0! (skipujem)");
            continue;
        }
        Console.WriteLine($"Postavljam novu nabavnu cenu {nabavnaCena.ToString("#,##0.00")}...");
        stavka.NabavnaCena = nabavnaCena;
        ctx.SaveChanges();
        Console.WriteLine("Azurirana cena!");
        PrintFooter();
        continue;
    }

}
Console.WriteLine("====");
Console.WriteLine("====");
Console.WriteLine("====");
Console.WriteLine();
Console.WriteLine("Gotovo!");
Console.WriteLine("Pritisni bilo koje dugme da izadjes!");
Console.WriteLine();
Console.WriteLine("====");
Console.WriteLine("====");
Console.WriteLine("====");
Console.Read();
