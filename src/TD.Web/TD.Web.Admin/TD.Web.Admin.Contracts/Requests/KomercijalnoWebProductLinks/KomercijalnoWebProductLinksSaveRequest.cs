using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks
{
    public class KomercijalnoWebProductLinksSaveRequest : LSCoreSaveRequest
    {
        public long RobaId { get; set; }
        public long WebId { get; set; }
    }
}
