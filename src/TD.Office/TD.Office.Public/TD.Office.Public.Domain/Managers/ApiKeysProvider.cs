using LSCore.Auth.Key.Contracts;
using Microsoft.Extensions.Configuration;

namespace TD.Office.Public.Domain.Managers;

public class ApiKeysProvider(IConfigurationRoot configurationRoot) : ILSCoreAuthKeyProvider
{
	public bool IsValidKey(string key)
	{
		var validKeys = configurationRoot["API_KEYS"].Split(",");
		return validKeys.Contains(key);
	}
}
