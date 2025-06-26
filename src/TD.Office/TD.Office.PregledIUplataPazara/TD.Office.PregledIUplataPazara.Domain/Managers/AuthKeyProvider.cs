using LSCore.Auth.Key.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TD.Office.PregledIUplataPazara.Domain.Managers;

public class AuthKeyProvider(IConfigurationRoot configurationRoot) : ILSCoreAuthKeyProvider
{
	public bool IsValidKey(string key)
	{
#if DEBUG
		return true;
#else
		var validKeys = configurationRoot["API_KEYS"].Split(",");
		return validKeys.Contains(key);
#endif
	}
}
