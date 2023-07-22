using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.Ping;
using TD.FE.TDOffice.Contracts.Requests.Ping;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IPingManager
    {
        Response<GetPingDto> Get(PingGetRequest request);
        Response Put(PingPutRequest request);
    }
}
