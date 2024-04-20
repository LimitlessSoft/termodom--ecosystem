namespace TD.Web.Common.Contracts.Requests.Users
{
    public class SendBulkSmsRequest
    {
        public string Text { get; set; }
        public int? FavoriteStoreId { get; set; }
        public int? ProfessionId { get; set; }
        public int? CityId { get; set; }
        public int? UserTypeId { get; set; }
        public bool? IsActive { get; set; }
    }
}