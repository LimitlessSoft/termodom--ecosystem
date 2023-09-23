namespace TD.Core.Contracts.Entities
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
        public bool IsActive {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
