using LSCore.Mapper.Contracts;
using TD.Office.MassSMS.Contracts.Dtos;
using TD.Office.MassSMS.Contracts.Entities;

namespace TD.Office.MassSMS.Contracts.DtoMappers;

public class SMSEntitySMSDtoMapper : ILSCoreMapper<SMSEntity, SMSDto>
{
	public SMSDto ToMapped(SMSEntity source) =>
		new() { PhoneNumber = source.Phone, Text = source.Text };
}
