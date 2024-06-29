using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Requests.Procedure;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IProcedureManager
    {
        double GetProdajnaCenaNaDan(ProceduraGetProdajnaCenaNaDanRequest request);
        List<NabavnaCenaNaDanDto> GetNabavnaCenaNaDan(ProceduraGetNabavnaCenaNaDanRequest request);
        List<ProdajnaCenaNaDanDto> GetProdajnaCenaNaDanOptimized(ProceduraGetProdajnaCenaNaDanOptimizedRequest request);
    }
}
