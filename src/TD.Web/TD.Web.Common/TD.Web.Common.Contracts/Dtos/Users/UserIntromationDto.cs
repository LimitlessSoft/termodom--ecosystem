using TD.Web.Common.Contracts.Enums;
using TD.Web.Public.Contrats.Dtos.Users;

namespace TD.Web.Common.Contracts.Dtos.Users
{
    public class UserInformationDto
    {
        public bool IsLogged { get; set; }
        public PurchaseMode PurchaseMode { get; set; }
        public UserDataDto UserData { get; set; }
    }
}
