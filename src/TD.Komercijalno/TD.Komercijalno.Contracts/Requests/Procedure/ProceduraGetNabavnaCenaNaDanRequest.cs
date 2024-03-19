namespace TD.Komercijalno.Contracts.Requests.Procedure
{
    public class ProceduraGetNabavnaCenaNaDanRequest
    {
        public required int MagacinId { get; set; }
        public required DateTime Datum { get; set; }
        
        public List<int>? RobaId { get; set; }
    }
}