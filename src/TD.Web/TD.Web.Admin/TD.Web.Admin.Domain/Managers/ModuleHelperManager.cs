using LSCore.Auth.Contracts;
using LSCore.Exceptions;
using TD.Web.Admin.Contracts.Dtos.ModuleHelper;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ModuleHelp;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class ModuleHelperManager(
	IModuleHelpRepository moduleHelpRepository,
	IUserRepository userRepository,
	LSCoreAuthContextEntity<string> contextEntity
) : IModuleHelperManager
{
	public ModuleHelpDto GetModuleHelps(GetModuleHelpRequest request)
	{
		if (contextEntity.IsAuthenticated == false)
			throw new LSCoreUnauthenticatedException();

		var currentUser = userRepository.GetCurrentUser();
		var userHelp = moduleHelpRepository.GetOrDefault(request.Module, currentUser.Id);
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
		if (contextEntity.IsAuthenticated == false)
			throw new LSCoreUnauthenticatedException();

		var currentUser = userRepository.GetCurrentUser();
		var entity = moduleHelpRepository.GetOrDefault(request.Module, currentUser.Id);

		if (entity == null)
			moduleHelpRepository.Insert(
				new ModuleHelpEntity
				{
					ModuleType = request.Module,
					Text = request.Text,
					CreatedBy = currentUser.Id
				}
			);
		else
		{
			entity.Text = request.Text;
			moduleHelpRepository.Update(entity);
		}
	}
}
