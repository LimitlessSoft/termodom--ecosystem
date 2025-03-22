using LSCore.Auth.UserPass.Contracts;
using LSCore.Auth.UserPass.Domain;

namespace TD.Office.Public.Domain.Managers;

public class AuthManager(
	LSCoreAuthUserPassConfiguration authUserPassConfiguration,
	ILSCoreAuthUserPassIdentityEntityRepository<string> authUserPassIdentityEntityRepository
)
	: LSCoreAuthUserPassManager<string>(
		authUserPassIdentityEntityRepository,
		authUserPassConfiguration
	);
