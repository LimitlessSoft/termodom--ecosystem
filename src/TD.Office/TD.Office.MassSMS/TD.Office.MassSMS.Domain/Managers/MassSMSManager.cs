using Hangfire;
using LSCore.Exceptions;
using LSCore.Validation.Contracts;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Logging;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Enums;
using TD.Office.MassSMS.Contracts.Enums.ValidationCodes;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Interfaces.Repositories;
using TD.Office.MassSMS.Contracts.Requests;
using TD.OfficeServer.Client;
using TD.OfficeServer.Contracts.Requests.SMS;

namespace TD.Office.MassSMS.Domain.Managers;

public class MassSMSManager(
	ILogger<MassSMSManager> logger,
	ISettingRepository settingRepository,
	IBackgroundJobClient backgroundJobClient,
	TDOfficeServerClient officeServerClient,
	ISMSRepository smsRepository
) : IMassSMSManager
{
	public void InvokeSending() => BackgroundJob.Enqueue(() => InvokeSendingJob());

	public void Queue(QueueSmsRequest request)
	{
		request.Validate();
		smsRepository.Insert(new SMSEntity { Phone = request.PhoneNumber, Text = request.Message });
	}

	public string GetCurrentStatus() => settingRepository.GetGlobalState().ToString();

	public int GetQueueCount() => smsRepository.GetMultiple().Count();

	#region Backgorund Jobs
	public void InvokeSendingJob()
	{
		logger.LogInformation("Invoking sending job.");
		var globalState = settingRepository.GetGlobalState();
		logger.LogInformation("Verify global state.");
		if (globalState != GlobalState.Initial)
			throw new LSCoreBadRequestException(
				SMSValidationCodes.SVC_007.GetValidationMessage(globalState)
			);
		logger.LogInformation("Querying SMSes.");
		var smses = smsRepository.GetMultiple();
		logger.LogInformation("Checking if there are any SMSes.");
		if (!smses.Any())
			throw new LSCoreBadRequestException(SMSValidationCodes.SVC_008.GetValidationMessage());

		logger.LogInformation("Update global state.");
		settingRepository.SetGlobalState(GlobalState.Sending);

		logger.LogInformation("Queueing SMSes.");
		foreach (var sms in smses.ToList())
			backgroundJobClient.Enqueue(() => SendSMS(sms));
	}

	public void SendSMS(SMSEntity sms)
	{
		officeServerClient
			.SendSMSAsync(new SMSQueueRequest { Text = sms.Text, Numbers = [sms.Phone] })
			.GetAwaiter()
			.GetResult();

		smsRepository.HardDelete(sms);

		if (!smsRepository.GetMultiple().Any())
			settingRepository.SetGlobalState(GlobalState.Initial);
	}
	#endregion
}
