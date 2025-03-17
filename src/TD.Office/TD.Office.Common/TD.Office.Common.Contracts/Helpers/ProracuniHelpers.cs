using LSCore.Exceptions;
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
				if (
					!userEntity.Permissions.Any(x =>
						x.IsActive && x.Permission == Permission.ProracuniMPForwardToKomercijalno
					)
				)
					throw new LSCoreForbiddenException();
				break;
			case ProracunType.NalogZaUtovar:
				if (
					!userEntity.Permissions.Any(x =>
						x.IsActive
						&& x.Permission == Permission.ProracuniNalogZaUtovarForwardToKomercijalno
					)
				)
					throw new LSCoreForbiddenException();
				break;
			case ProracunType.Veleprodajni:
				if (
					!userEntity.Permissions.Any(x =>
						x.IsActive && x.Permission == Permission.ProracuniVPForwardToKomercijalno
					)
				)
					throw new LSCoreForbiddenException();
				break;
			default:
				throw new LSCoreForbiddenException();
		}
	}
}
