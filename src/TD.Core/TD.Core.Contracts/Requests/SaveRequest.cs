namespace TD.Core.Contracts.Requests
{
    public class SaveRequest
    {
        public int? Id { get; set; }

        public SaveRequest()
        {

        }

        public SaveRequest(int? Id)
        {
            this.Id = Id;
        }
    }
}
