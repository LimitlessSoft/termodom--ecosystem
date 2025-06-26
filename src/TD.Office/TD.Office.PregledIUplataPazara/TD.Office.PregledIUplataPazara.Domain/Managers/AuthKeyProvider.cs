using LSCore.Auth.Key.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TD.Office.PregledIUplataPazara.Domain.Managers;

public class AuthKeyProvider(IConfigurationRoot configurationRoot, ILogger<AuthKeyProvider> logger)
	: ILSCoreAuthKeyProvider
{
	public bool IsValidKey(string key)
	{
		logger.LogInformation("Validating API key: {0}", key);
		logger.LogInformation(
			"Validating against configuration: {0}",
			configurationRoot["API_KEYS"]
		);
#if DEBUG
		return true;
#else
		var validKeys = configurationRoot["API_KEYS"].Split(",");
		return validKeys.Contains(key);
#endif
	}
}
