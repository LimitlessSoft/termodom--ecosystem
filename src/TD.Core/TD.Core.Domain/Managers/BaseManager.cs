using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using System.Linq.Expressions;
using TD.Core.Contracts;
using TD.Core.Contracts.Requests;

namespace TD.Core.Domain.Managers
{
    public class BaseManager<TManager>
    {
        private readonly ILogger<TManager> _logger;
        private readonly DbContext? _dbContext;

        public BaseManager(ILogger<TManager> logger)
        {
            _logger = logger;
            _dbContext = null;
        }

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
        public TEntity Save<TEntity, TRequest>(TRequest request, Func<TRequest, IEntity?, TEntity>? requestMapping = null)
            where TEntity : class, IEntity, new()
            where TRequest : SaveRequest
        {
            if (_dbContext == null)
                throw new ArgumentNullException(nameof(_dbContext));

            var entity = new TEntity();
            if (!request.Id.HasValue)
            {
                var lastId = _dbContext.Set<TEntity>()
                    .AsQueryable()
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (requestMapping == null)
                    entity.InjectFrom(request);
                else
                    entity = requestMapping(request, null);

                entity.Id = ++lastId;

                _dbContext.Set<TEntity>()
                    .Add(entity);
            }
            else
            {
                entity = _dbContext.Set<TEntity>()
                    .FirstOrDefault(x => x.Id == request.Id);

                if (entity == null)
                    return null;

                if (requestMapping == null)
                    entity.InjectFrom(request);
                else
                    entity = requestMapping(request, entity);

                _dbContext.Set<TEntity>()
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
            if (_dbContext == null)
                throw new ArgumentNullException(nameof(_dbContext));

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
            if (_dbContext == null)
                throw new ArgumentNullException(nameof(_dbContext));

            return Queryable<T>().First(predicate);
        }

        /// <summary>
        /// Gets first or default T entity
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T? FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (_dbContext == null)
                throw new ArgumentNullException(nameof(_dbContext));

            return Queryable<T>().FirstOrDefault(predicate);
        }
    }

    public class BaseManager<TManager, TEntity> : BaseManager<TManager> where TEntity : class, IEntity, new()
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
        public TEntity Save<TRequest>(TRequest request, Func<TRequest, IEntity?, TEntity>? requestMapping = null)
            where TRequest : SaveRequest
        {
            return Save<TEntity, TRequest>(request, requestMapping);
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
