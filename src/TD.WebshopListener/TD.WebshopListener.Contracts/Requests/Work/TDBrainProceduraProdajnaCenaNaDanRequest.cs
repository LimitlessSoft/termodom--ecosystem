namespace TD.WebshopListener.Contracts.Requests.Work
{
    public class TDBrainProceduraProdajnaCenaNaDanRequest
    {
        public int BazaId { get; set; }
        public int GodinaBaze { get; set; }
        public int MagacinId { get; set; }
        public int RobaId { get; set; }
        public DateTime Datum { get; set; }
    }
}
