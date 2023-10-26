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

        public bool IsNew { get => !Id.HasValue; }

        public bool IsOld { get => Id.HasValue; }
    }
}
