using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks
{
    public class KomercijalnoWebProductLinksSaveRequest : LSCoreSaveRequest
    {
        public int RobaId { get; set; }
        public long WebId { get; set; }
    }
}
