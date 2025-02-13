using LSCore.Contracts.Configurations;
using LSCore.Contracts.Interfaces.Repositories;
using LSCore.Domain.Managers;

namespace TD.Web.Common.Domain.Managers;

public class AuthManager(
    LSCoreAuthorizationConfiguration authorizationConfiguration,
    ILSCoreAuthorizableEntityRepository authorizableEntityRepository
) : LSCoreAuthorizeManager(authorizationConfiguration, authorizableEntityRepository);
