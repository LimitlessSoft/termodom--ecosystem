namespace TD.Office.Public.Contracts.Requests.NalogZaPrevoz
{
    public class GetMultipleNalogZaPrevozRequest
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int StoreId { get; set; }
    }
}