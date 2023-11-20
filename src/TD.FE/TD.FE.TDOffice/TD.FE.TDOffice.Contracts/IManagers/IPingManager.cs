using LSCore.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.Ping;
using TD.FE.TDOffice.Contracts.Requests.Ping;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IPingManager
    {
        LSCoreResponse<GetPingDto> Get(PingGetRequest request);
        LSCoreResponse Put(PingPutRequest request);
    }
}
