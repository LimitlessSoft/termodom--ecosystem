using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.TDOffice
{
    public partial class SpecifikacijaNovca
    {
        public class Detalji
        {
            public KursBlok Kurs1 { get; set; } = new KursBlok();
            public KursBlok Kurs2 { get; set; } = new KursBlok();
            public double Novcanice5000 { get; set; } = 0;
            public double Novcanice2000 { get; set; } = 0;
            public double Novcanice1000 { get; set; } = 0;
            public double Novcanice500 { get; set; } = 0;
            public double Novcanice200 { get; set; } = 0;
            public double Novcanice100 { get; set; } = 0;
            public double Novcanice50 { get; set; } = 0;
            public double Novcanice20 { get; set; } = 0;
            public double Novcanice10 { get; set; } = 0;
            public double Novcanice5 { get; set; } = 0;
            public double Novcanice2 { get; set; } = 0;
            public double Novcanice1 { get; set; } = 0;
            public double Kartice { get; set; } = 0;
            public double Papiri { get; set; } = 0;
            public double Troskovi { get; set; } = 0;
            public double VozaciDuguju { get; set; } = 0;
            public double KodSase { get; set; } = 0;
            public string Beleksa { get; set; }
            public string KarticeBeleksa { get; set; }
            public string CekoviBeleksa { get; set; }
            public string PapiriBeleksa { get; set; }
            public string TroskoviBeleksa { get; set; }
            public string VozaciDugujuBeleksa { get; set; }
            public string KodSaseBeleksa { get; set; }
            public double DnevniIzvestaj { get; set; } = 0;
            /// <summary>
            /// Brojevi racuna koji su stornirani 
            /// </summary>
            public List<int> StorniraniMPRacuni { get; set; } = new List<int>();
            /// <summary>
            /// Brojevi racuna koji su stornirani u fiskalnoj kasi po osnovu duple fiskalizacije
            /// </summary>
            public List<int> StornoKasaDuplo { get; set; } = new List<int>();
            public double Zbir()
            {
                return
                    Kurs1.Vrednost() +
                    Kurs2.Vrednost() +
                    Novcanice5000 * 5000 +
                    Novcanice2000 * 2000 +
                    Novcanice1000 * 1000 +
                    Novcanice500 * 500 +
                    Novcanice200 * 200 +
                    Novcanice100 * 100 +
                    Novcanice50 * 50 +
                    Novcanice20 * 20 +
                    Novcanice10 * 10 +
                    Novcanice5 * 5 +
                    Novcanice2 * 2 +
                    Novcanice1 +
                    Kartice +
                    Papiri +
                    Troskovi +
                    KodSase +
                    VozaciDuguju
                    ;
            }
        }
    }
}
