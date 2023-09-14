namespace TD.Core.Contracts.Requests
{
    public class IdRequest
    {
        public int Id { get; set; }

        public IdRequest()
        {

        }
        public IdRequest(int id)
        {
            Id = id;
        }
    }
}
