using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Requests.Procedure;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IProcedureManager
    {
        Response<double> GetProdajnaCenaNaDan(ProceduraGetProdajnaCenaNaDanRequest request);
    }
}
