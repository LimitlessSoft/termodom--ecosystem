using LSCore.SortAndPage.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Enums.SortColumnCodes;

public static class TicketsSortColumnCodes
{
	public enum Tickets
	{
		CreatedAt,
		Type,
		Status,
		Priority
	}

	public static Dictionary<Tickets, LSCoreSortRule<TicketEntity>> TicketsSortRules = new()
	{
		{ Tickets.CreatedAt, new LSCoreSortRule<TicketEntity>(x => x.CreatedAt) },
		{ Tickets.Type, new LSCoreSortRule<TicketEntity>(x => x.Type) },
		{ Tickets.Status, new LSCoreSortRule<TicketEntity>(x => x.Status) },
		{ Tickets.Priority, new LSCoreSortRule<TicketEntity>(x => x.Priority) },
	};
}
