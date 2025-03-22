using LSCore.Common.Extensions;
using LSCore.Exceptions;
using TD.Office.Common.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Contracts.Helpers;

public static class ProcessInProgressHelpers
{
	/// <summary>
	/// Validates that the process is not in progress.
	/// If it is in progress, throws <see cref="LSCoreBadRequestException"/>.
	/// Otherwise, sets the process as in progress.
	/// </summary>
	/// <param name="cacheManager"></param>
	/// <param name="process"></param>
	/// <exception cref="LSCoreBadRequestException"></exception>
	public static async Task ValidateProcessIsNotInProgressAsync(
		ICacheManager cacheManager,
		string process
	)
	{
		var cachedData = await cacheManager.GetDataAsync<string>(process);

		if (!string.IsNullOrEmpty(cachedData))
			throw new LSCoreBadRequestException(WebValidationCodes.WVC_001.GetDescription()!);

		await cacheManager.SetDataAsync<string>(
			process,
			() => "In progress",
			TimeSpan.FromMinutes(2)
		);
	}

	public static async Task SetProcessAsCompletedAsync(
		ICacheManager cacheManager,
		string process
	) => await cacheManager.RemoveDataAsync(process);
}
