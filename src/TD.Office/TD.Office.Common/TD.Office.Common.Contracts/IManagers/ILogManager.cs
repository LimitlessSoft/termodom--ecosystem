using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.IManagers;

public interface ILogManager
{
	void Log(LogKey key);
	void Log(LogKey key, string value);
}
