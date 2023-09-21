namespace TD.Core.Contracts
{
    public interface IMap<TEntity, TRequest> where TEntity : IEntityBase
    {
        void Map(TEntity entity, TRequest request);
    }
}
