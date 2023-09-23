using TD.Core.Contracts.Requests;

namespace TD.Web.Contracts.Requests.Units
{
    public class UnitSaveRequest : SaveRequest
    {
        public string Name { get; set; }
    }
}
