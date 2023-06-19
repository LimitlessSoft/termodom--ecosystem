namespace TD.Komercijalno.Contracts.Requests.Procedure
{
    public class ProceduraGetProdajnaCenaNaDanRequest
    {
        public int MagacinId { get; set; }
        public int RobaId { get; set; }
        public DateTime Datum { get; set; }
    }
}
