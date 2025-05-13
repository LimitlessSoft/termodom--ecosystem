namespace TD.Core.Contracts
{
	public interface IEntity : IEntityBase
	{
		bool IsActive { get; set; }
		DateTime CreatedAt { get; set; }
		int CreatedBy { get; set; }
		int? UpdatedBy { get; set; }
		DateTime? UpdatedAt { get; set; }
	}
}
