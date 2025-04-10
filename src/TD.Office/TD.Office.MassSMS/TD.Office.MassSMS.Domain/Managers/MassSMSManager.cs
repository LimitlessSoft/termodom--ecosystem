using System.Collections.Concurrent;
using Hangfire;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Contracts;
using LSCore.Validation.Domain;
using Microsoft.Extensions.Logging;
using TD.Office.MassSMS.Contracts.Dtos;
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
	ISMSRepository smsRepository,
	IPhoneValidatorSRB phoneValidatorSrb
) : IMassSMSManager
{
	public void InvokeSending() => BackgroundJob.Enqueue(() => InvokeSendingJob());

	public void Queue(QueueSmsRequest request)
	{
		request.Validate();
		smsRepository.Insert(
			new SMSEntity
			{
				Phone = phoneValidatorSrb.Format(request.PhoneNumber),
				Text = request.Message
			}
		);
	}

	public string GetCurrentStatus() => settingRepository.GetGlobalState().ToString();

	public int GetQueueCount() => smsRepository.GetMultiple().Count();

	public List<SMSDto> GetQueue() => smsRepository.GetMultiple().ToMappedList<SMSEntity, SMSDto>();

	public void ClearQueue()
	{
		smsRepository.HardDelete(smsRepository.GetMultiple());
	}

	public void MassQueue(MassQueueSmsRequest request)
	{
		var ins = new ConcurrentBag<SMSEntity>();
		request.PhoneNumbers = request
			.PhoneNumbers.Where(x => !string.IsNullOrWhiteSpace(x))
			.ToList();
		Parallel.ForEach(
			request.PhoneNumbers,
			item =>
			{
				try
				{
					var singleRequest = new QueueSmsRequest
					{
						PhoneNumber = item,
						Message = request.Message
					};
					singleRequest.Validate();
					ins.Add(
						new SMSEntity
						{
							Phone = phoneValidatorSrb.Format(item),
							Text = request.Message
						}
					);
				}
				catch
				{
					// Maybe should return how many not actually added
				}
			}
		);

		smsRepository.Insert(ins);
	}

	public void ClearDuplicates() => smsRepository.ClearDuplicates();

	public void SetText(SetTextRequest request)
	{
		request.Validate();
		smsRepository.SetText(request.Text);
	}

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
		var groupped = smses.GroupBy(x => x.Text).ToList();

		foreach (var group in groupped)
			SendSMS(group.ToList());
	}

	public void SendSMS(List<SMSEntity> smsS)
	{
		// all entities have same text
		officeServerClient
			.SendSMSAsync(
				new SMSQueueRequest
				{
					Text = smsS.First().Text,
					Numbers = smsS.Select(x => x.Phone).ToList()
				}
			)
			.GetAwaiter()
			.GetResult();

		smsRepository.HardDelete(smsS);

		if (!smsRepository.GetMultiple().Any())
			settingRepository.SetGlobalState(GlobalState.Initial);
	}
	#endregion
}
