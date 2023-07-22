using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TD.Core.Contracts;

namespace TD.Core.Domain.Managers
{
    public class BaseManager<TManager>
    {
        private readonly ILogger<TManager> _logger;
        private readonly DbContext _dbContext;

        public BaseManager(ILogger<TManager> logger, DbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds or updates entity to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Save<T>(T entity) where T : class, IEntity
        {
            if (entity.Id == 0)
            {
                var lastId = _dbContext.Set<T>()
                    .AsQueryable()
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                entity.Id = ++lastId;

                _dbContext.Set<T>()
                    .Add(entity);
            }
            else
            {
                _dbContext.Set<T>()
                    .Update(entity);
            }
            _dbContext.SaveChanges();
            return entity;
        }

        /// <summary>
        /// Gets T entity table as queryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> Queryable<T>() where T : class
        {
            return _dbContext.Set<T>()
                .AsQueryable();
        }

        /// <summary>
        /// Gets first T entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T First<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Queryable<T>().First(predicate);
        }

        /// <summary>
        /// Gets first or default T entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T? FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Queryable<T>().FirstOrDefault(predicate);
        }
    }

    public class BaseManager<TManager, TEntity> : BaseManager<TManager> where TEntity : class, IEntity
    {
        private readonly ILogger<TManager> _logger;
        private readonly DbContext _dbContext;

        public BaseManager(ILogger<TManager> logger, DbContext dbContext)
            :base(logger, dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds or save entity to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Save(TEntity entity)
        {
            return Save<TEntity>(entity);
        }

        /// <summary>
        /// Gets manager entity table as queryable
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Queryable()
        {
            return Queryable<TEntity>();
        }

        /// <summary>
        /// Gets first manager entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return First<TEntity>(predicate);
        }

        /// <summary>
        /// Gets first or default manager entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return FirstOrDefault<TEntity>(predicate);
        }
    }
}
