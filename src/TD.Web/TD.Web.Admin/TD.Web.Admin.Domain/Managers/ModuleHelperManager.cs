using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using TD.Web.Admin.Contracts.Dtos.ModuleHelper;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ModuleHelp;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class ModuleHelperManager(
    IModuleHelpRepository moduleHelpRepository,
    LSCoreContextUser contextUser
) : IModuleHelperManager
{
    public ModuleHelpDto GetModuleHelps(GetModuleHelpRequest request)
    {
        if (contextUser is not { Id: not null })
            throw new LSCoreUnauthenticatedException();

        var userHelp = moduleHelpRepository.GetOrDefault(request.Module, contextUser.Id.Value);
        var systemHelp = moduleHelpRepository.GetOrDefault(request.Module, 0);

        return new ModuleHelpDto
        {
            UserText = userHelp?.Text ?? string.Empty,
            SystemText = systemHelp?.Text ?? string.Empty,
            UserHelpId = userHelp?.Id,
            SystemHelpId = systemHelp?.Id
        };
    }

    public void PutModuleHelps(PutModuleHelpRequest request)
    {
        if (contextUser is not { Id: not null })
            throw new LSCoreUnauthenticatedException();

        var entity = moduleHelpRepository.GetOrDefault(request.Module, contextUser.Id.Value);

        if (entity == null)
            moduleHelpRepository.Insert(new ModuleHelpEntity { ModuleType = request.Module, Text = request.Text });
        else
        {
            entity.Text = request.Text;
            moduleHelpRepository.Update(entity);
        }
    }
}
