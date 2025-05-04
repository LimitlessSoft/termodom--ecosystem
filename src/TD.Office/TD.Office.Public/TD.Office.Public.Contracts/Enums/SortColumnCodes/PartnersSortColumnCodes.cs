using System.Linq.Expressions;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Office.Public.Contracts.Enums.SortColumnCodes;

public static class PartnersSortColumnsCodes
{
	public enum Partners
	{
		Ppid
	}

	public static Dictionary<Partners, Expression<Func<Partner, object>>> PartnersSortRules =
		new() { { Partners.Ppid, x => x.Ppid } };
}
