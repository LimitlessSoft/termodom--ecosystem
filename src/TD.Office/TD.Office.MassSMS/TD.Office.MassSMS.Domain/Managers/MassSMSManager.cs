using Hangfire;
using LSCore.Exceptions;
using Microsoft.Extensions.Logging;
using TD.Office.MassSMS.Contracts.Enums;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Interfaces.Repositories;

namespace TD.Office.MassSMS.Domain.Managers;

public class MassSMSManager(
	ILogger<MassSMSManager> logger,
	ISettingRepository settingRepository,
	IBackgroundJobClient backgroundJobClient
) : IMassSMSManager
{
	// public void InvokeSending() => BackgroundJob.Enqueue(() => InvokeSendingJob());
	public void InvokeSending()
	{
		for (var i = 0; i < 100; i++)
			backgroundJobClient.Enqueue(() => Asd());
	}

	#region Backgorund Jobs

	public void Asd()
	{
		logger.LogInformation("Asd - start");
		Thread.Sleep(10_000);
		logger.LogInformation("Asd - end");
	}

	public void InvokeSendingJob()
	{
		logger.LogInformation("Invoking sending job.");
		var globalState = settingRepository.GetGlobalState();
		logger.LogInformation("Verify global state.");
		if (globalState != GlobalState.Initial)
			throw new LSCoreBadRequestException(
				"Masovno slanje SMS je vec zapoceto, ne mozete pokrenutni novi dok se prethodni ne zavrsi."
			);

		logger.LogInformation("Update global state.");
		settingRepository.SetGlobalState(GlobalState.Sending);
		logger.LogInformation("Pokrenutno masovo slanje SMS-a.");
	}
	#endregion
}
