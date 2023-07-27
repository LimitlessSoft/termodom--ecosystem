using TD.Core.Contracts.Http;

namespace TD.FE.TDOffice.Contracts.IManagers
{
    public interface IWebUredjivanjeProizvodaManager
    {
        Response<string> KomercijalnoRobaGet();
    }
}
