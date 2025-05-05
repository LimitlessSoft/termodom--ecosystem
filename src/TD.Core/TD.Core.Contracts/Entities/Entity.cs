namespace TD.Core.Contracts.Entities
{
	public abstract class Entity : IEntity
	{
		public int Id { get; set; }
		public bool IsActive { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedAt { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
	}
}
