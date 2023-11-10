using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Units
{
    public class UnitSaveRequest : LSCoreSaveRequest
    {
        public string Name { get; set; }
    }
}
