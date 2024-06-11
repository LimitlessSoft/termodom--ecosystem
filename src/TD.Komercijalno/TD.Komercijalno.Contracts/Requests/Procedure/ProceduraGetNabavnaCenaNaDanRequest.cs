namespace TD.Komercijalno.Contracts.Requests.Procedure
{
    public class ProceduraGetNabavnaCenaNaDanRequest
    {
        public required DateTime Datum { get; set; }
        
        public List<long>? RobaId { get; set; }
    }
}