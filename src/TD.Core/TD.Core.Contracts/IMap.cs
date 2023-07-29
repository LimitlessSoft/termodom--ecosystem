namespace TD.Core.Contracts
{
    public interface IMap<TEntity, TRequest> where TEntity : IEntity
    {
        void Map(TEntity entity, TRequest request);
    }
}
