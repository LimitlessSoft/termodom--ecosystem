using LSCore.Repository.Contracts;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Enums;

namespace TD.Office.MassSMS.Contracts.Interfaces.Repositories;

public interface ISettingRepository : ILSCoreRepositoryBase<SettingEntity>
{
	GlobalState GetGlobalState();
	void SetGlobalState(GlobalState state);
}
