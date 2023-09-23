namespace TD.Core.Contracts
{
    public interface IEntity : IEntityBase
    {
        bool is_active { get; set; }
        DateTime created_at { get; set; }
        long? updated_by { get; set; }
        DateTime? updated_at { get; set; }
    }
}
