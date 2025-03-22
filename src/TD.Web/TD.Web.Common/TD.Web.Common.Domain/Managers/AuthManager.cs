using LSCore.Auth.UserPass.Contracts;
using LSCore.Auth.UserPass.Domain;

namespace TD.Web.Common.Domain.Managers;

public class AuthManager(
	ILSCoreAuthUserPassIdentityEntityRepository<string> userPassIdentityEntityRepository,
	LSCoreAuthUserPassConfiguration userPassConfiguration
) : LSCoreAuthUserPassManager<string>(userPassIdentityEntityRepository, userPassConfiguration);
