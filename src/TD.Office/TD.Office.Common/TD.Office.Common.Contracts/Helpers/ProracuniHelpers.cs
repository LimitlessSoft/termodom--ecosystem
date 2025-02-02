using LSCore.Contracts.Exceptions;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Helpers;
public static class ProracuniHelpers
{
    public static void HasPermissionToForwad(UserEntity userEntity, ProracunType proracunType)
    {
        switch (proracunType)
        {
            case ProracunType.Maloprodajni:
                if (!userEntity.Permissions.Any(x => x.IsActive && x.Permission == Common.Contracts.Enums.Permission.ProracuniMPForwardToKomercijalno))
                    throw new LSCoreForbiddenException();
                break;
            case ProracunType.NalogZaUtovar:
                if (!userEntity.Permissions.Any(x => x.IsActive && x.Permission == Common.Contracts.Enums.Permission.ProracuniNalogZaUtovarForwardToKomercijalno))
                    throw new LSCoreForbiddenException();
                break;
            case ProracunType.Veleprodajni:
                if (!userEntity.Permissions.Any(x => x.IsActive && x.Permission == Common.Contracts.Enums.Permission.ProracuniVPForwardToKomercijalno))
                    throw new LSCoreForbiddenException();
                break;
            default:
                throw new LSCoreForbiddenException();
        }
    }
}
