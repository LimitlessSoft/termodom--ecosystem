using LSCore.Repository;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Enums;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Interfaces.Repositories;

namespace TD.Office.MassSMS.Repository.Repositories;

public class SettingRepository(IMassSMSContext dbContext)
	: LSCoreRepositoryBase<SettingEntity>(dbContext),
		ISettingRepository
{
	public GlobalState GetGlobalState() => Enum.Parse<GlobalState>(GetMultiple().First().Value);

	public void SetGlobalState(GlobalState state)
	{
		var setting = GetMultiple().First(x => x.Setting == Setting.GlobalState);
		setting.Value = state.ToString();
		Update(setting);
	}
}
