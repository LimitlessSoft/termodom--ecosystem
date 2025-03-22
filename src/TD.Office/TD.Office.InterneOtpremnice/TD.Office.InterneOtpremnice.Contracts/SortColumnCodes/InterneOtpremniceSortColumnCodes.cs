using LSCore.SortAndPage.Contracts;
using TD.Office.InterneOtpremnice.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Contracts.SortColumnCodes;

public static class InterneOtpremniceSortColumnCodes
{
	public enum InterneOtpremnice
	{
		Id
	}

	public static Dictionary<InterneOtpremnice, LSCoreSortRule<InternaOtpremnicaEntity>> Rules =>
		new() { { InterneOtpremnice.Id, new LSCoreSortRule<InternaOtpremnicaEntity>(x => x.Id) } };
}
