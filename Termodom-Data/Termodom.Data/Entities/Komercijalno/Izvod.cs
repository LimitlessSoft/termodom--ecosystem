namespace Termodom.Data.Entities.Komercijalno
{
    public class Izvod
    {
        public int IzvodId { get; set; }
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public int PPID { get; set; }
        public string SifPlac { get; set; }
        public double Duguje { get; set; }
        public double Potrazuje { get; set; }
        public int Rasporedjen { get; set; }
        public double RDuguje { get; set; }
        public double RPotrazuje { get; set; }
        public int R { get; set; }
        public string Konto { get; set; }
        public string Valuta { get; set; }
        public double Kurs { get; set; }
        public string PozNaBroj { get; set; }
        public string ZiroRacun { get; set; }
        public int UPPID { get; set; }
        public string Svrha { get; set; }
        public string ED1 { get; set; }
        public string PozNaBrojOd { get; set; }
        public int? VirmanID { get; set; }
        public string PoPDVBroj { get; set; }
    }
}
