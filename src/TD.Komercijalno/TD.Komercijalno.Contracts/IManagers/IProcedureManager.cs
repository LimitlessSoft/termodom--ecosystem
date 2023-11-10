using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Requests.Procedure;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IProcedureManager
    {
        LSCoreResponse<double> GetProdajnaCenaNaDan(ProceduraGetProdajnaCenaNaDanRequest request);
    }
}
