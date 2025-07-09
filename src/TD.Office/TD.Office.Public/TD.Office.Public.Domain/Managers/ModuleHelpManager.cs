using LSCore.Auth.Contracts;
using LSCore.Exceptions;
using TD.Office.Common.Contracts.Dtos.ModuleHelp;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Contracts.IRepositories;
using TD.Office.Common.Contracts.Requests.ModuleHelp;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Domain.Managers;

public class ModuleHelpManager(
	IModuleHelpRepository moduleHelpRepository,
	IUserRepository userRepository,
	LSCoreAuthContextEntity<string> contextEntity
) : IModuleHelpManager
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
