namespace TD.Core.Contracts.Entities
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
        public bool is_active {  get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public long? updated_by { get; set; }
        public DateTime? updated_at { get; set; }
    }
}
