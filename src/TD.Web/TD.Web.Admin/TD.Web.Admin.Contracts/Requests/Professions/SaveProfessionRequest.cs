using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Professions
{
    public class SaveProfessionRequest : LSCoreSaveRequest
    {
        public string Name { get; set; }
    }
}
