using StavkePocetnogStanjaFilteriRaznorazni;

var bazaConnString = "C:\\Poslovanje\\Baze\\2025\\FRANSIZA2025TCMD.FDB";
Console.WriteLine("===");
Console.WriteLine("Baza: " + bazaConnString);
Console.WriteLine("===");
Console.WriteLine("Selektujem stavke pocetnog stanje po sledecem filteru:");
Console.WriteLine("1. Stavka ima kolicinu, a razlika izmedju nabavne cene je manja ili veca od X%");
Console.WriteLine("0. Izadji");
Console.Write("Izaberi opciju: ");

string opt = string.Empty;
while (string.IsNullOrWhiteSpace(opt))
{
    opt = Console.ReadLine();
    switch (opt)
    {
        case "1":
            StartStavkePocetnogStanjaSaKolicinomIAbsolutnomRazlikomProdajneINabavneCene();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Nepoznata opcija!");
            break;
    }
}

// ===
void StartStavkePocetnogStanjaSaKolicinomIAbsolutnomRazlikomProdajneINabavneCene()
{
    var source = new DB($"data source=4monitor; initial catalog = {bazaConnString}; user=SYSDBA; password=m");
    var ctx = source.CreateContext();
    int vrDok = 0;
    int brDok = 0;
    double razlikaProcenti = 0;
    int znak = 1;
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
        var dok = ctx.Dokumenti.FirstOrDefault(x => x.VrDok == vrDok && x.BrDok == brDok);
        if(dok == null)
        {
            Console.WriteLine("Dokument nije pronadjen!");
            UnosBrojaDokumenta();
        }
    }
    void UnosZnaka()
    {
        Console.WriteLine();
        Console.WriteLine("Zelim da selektuje stavke kojima je razlika prodajne i nabavne cene: ");
        Console.WriteLine("1. Veci od X%");
        Console.WriteLine("2. Manji od X%");
        var opt = Console.ReadLine();
        int o = 0;
        if(!int.TryParse(opt, out o))
        {
            Console.WriteLine("Neispravna opcija!");
            UnosZnaka();
        }
        znak = o == 1 ? 1 : -1;
    }
    void UnosRazlikeProcenti()
    {
        Console.WriteLine();
        Console.Write("Unesite procenat razlike prodajne i nabavne cene: ");
        var b = Console.ReadLine();
        if(!double.TryParse(b, out razlikaProcenti))
        {
            Console.WriteLine("Procenat nije dobar!");
            UnosRazlikeProcenti();
        }
        if(razlikaProcenti <= 0)
        {
            Console.WriteLine("Unesite procenat u pozitivnoj vrednosti.");
            UnosRazlikeProcenti();
        }
    }
    Console.WriteLine();
    Console.WriteLine("Ova akcija ce selektovati i ispisati sve stavke pocetnog stanja kojima je razlika izmedju prodajne i nabavne cene veca ili manja od X%");
    UnosBrojaDokumenta();
    UnosZnaka();
    UnosRazlikeProcenti();

    var sifarnik = ctx.Roba.ToDictionary(x => x.Id, x => x);
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

    var stavke = ctx.Stavke.Where(x => x.VrDok == vrDok && x.BrDok == brDok && x.Kolicina != 0).ToList();
    foreach(var stavka in stavke)
    {
        if(stavka.ProdajnaCena == 0)
        {
            PrintHeader(stavka.RobaId);
            Console.WriteLine($"Prodajna cena 0!");
            PrintFooter();
            continue;
        }
        if(stavka.NabavnaCena == 0)
        {
            PrintHeader(stavka.RobaId);
            Console.WriteLine($"Nabavna cena 0!");
            PrintFooter();
            continue;
        }
        if(stavka.ProdajnaCena <= stavka.NabavnaCena)
        {
            PrintHeader(stavka.RobaId);
            Console.WriteLine($"Prodajna cena == Nabavna cena!");
            PrintFooter();
            continue;
        }
        var razlikaPerc = (stavka.ProdajnaCena / stavka.NabavnaCena - 1) * 100;
        if(znak == 1)
        {
            if(razlikaPerc > razlikaProcenti)
            {
                PrintHeader(stavka.RobaId);
                Console.WriteLine($"Razlika prodajne i nabavne cene veca od {razlikaProcenti}%!");
                Console.WriteLine($"Nabavna cena: {stavka.NabavnaCena}");
                Console.WriteLine($"Prodajna cena: {stavka.ProdajnaCena}");
                PrintFooter();
                continue;
            }
        }else
        {
            if (razlikaPerc < razlikaProcenti)
            {
                PrintHeader(stavka.RobaId);
                Console.WriteLine($"Razlika prodajne i nabavne cene manja od {razlikaProcenti}%!");
                Console.WriteLine($"Nabavna cena: {stavka.NabavnaCena}");
                Console.WriteLine($"Prodajna cena: {stavka.ProdajnaCena}");
                PrintFooter();
                continue;
            }
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
}