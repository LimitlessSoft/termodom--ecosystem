using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using TD.Core.Contracts;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Http.Interfaces;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Requests;

namespace TD.Core.Domain.Managers
{
    public class BaseManager<TManager> : IBaseManager
    {
        private readonly ILogger<TManager> _logger;
        private readonly DbContext? _dbContext;

        public ContextUser CurrentUser { get; set; }

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

        public void SetContextInfo(HttpContext httpContext)
        {
            var claims = httpContext.User?.Claims.ToList();
            if (claims == null)
                return;

            CurrentUser = new ContextUser();
            CurrentUser.Username = claims.FirstOrDefault(x => x.Type == Contracts.Constants.ClaimNames.CustomUsername)?.Value.ToString() ?? "UNDEFINED";
        }

        /// <summary>
        /// Adds or updates entity to database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Response<TEntity> Save<TEntity, TRequest>(TRequest request)
            where TEntity : class, IEntity, new()
            where TRequest : SaveRequest
        {
            if (_dbContext == null)
                return Response<TEntity>.InternalServerError(CommonValidationCodes.COMM_005.GetDescription(String.Empty));

            var entityMapper = (IMap<TEntity, TRequest>?)Constants.Container?.TryGetInstance(typeof(IMap<TEntity, TRequest>));

            var entity = new TEntity();
            if (!request.Id.HasValue)
            {
                var lastId = _dbContext.Set<TEntity>()
                    .AsQueryable()
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (entityMapper == null)
                    entity.InjectFrom(request);
                else
                    entityMapper.Map(entity, request);

                entity.Id = ++lastId;

                _dbContext.Set<TEntity>()
                    .Add(entity);
            }
            else
            {
                entity = _dbContext.Set<TEntity>()
                    .FirstOrDefault(x => x.Id == request.Id);

                if (entity == null)
                    return Response<TEntity>.NotFound();

                if (entityMapper == null)
                    entity.InjectFrom(request);
                else
                    entityMapper.Map(entity, request);

                entity.UpdatedAt = DateTime.UtcNow;

                _dbContext.Set<TEntity>()
                    .Update(entity);
            }
            try
            {
                _dbContext.SaveChanges();
            }
            catch
            {
                return Response<TEntity>.InternalServerError();
            }

            return new Response<TEntity>(entity);
        }

        /// <summary>
        /// Updates entity into database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public TEntity Update<TEntity>(TEntity entity) where TEntity : class
        {
            if (_dbContext == null)
                throw new ArgumentNullException(nameof(_dbContext));

            _dbContext.Set<TEntity>()
                .Update(entity);

            _dbContext.SaveChanges();

            return entity;
        }

        /// <summary>
        /// Inserts entity into database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            if (_dbContext == null)
                throw new ArgumentNullException(nameof(_dbContext));

            _dbContext.Set<TEntity>()
                .Add(entity);

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
        public Response<T> First<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            if (_dbContext == null)
                return Response<T>.InternalServerError(CommonValidationCodes.COMM_005.GetDescription(String.Empty));

            var entity = Queryable<T>().FirstOrDefault(predicate);
            if (entity == null)
                return Response<T>.NotFound();

            return new Response<T>(entity);
        }

        /// <summary>
        /// Deletes record from the database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void HardDelete<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext
                .Set<TEntity>()
                .Remove(entity);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Updates records is_active to false
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void SoftDelete<TEntity>(TEntity entity) where TEntity : class, IEntity
        {
            entity.IsActive = false;
            Update(entity);
        }
    }

    public class BaseManager<TManager, TEntity> : BaseManager<TManager> where TEntity
        : class, IEntity, new()
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
        public Response<TEntity> Save<TRequest>(TRequest request)
            where TRequest : SaveRequest
        {
            return Save<TEntity, TRequest>(request);
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
        public Response<TEntity> First(Expression<Func<TEntity, bool>> predicate)
        {
            return First<TEntity>(predicate);
        }

        /// <summary>
        /// Deletes record from the database
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        public void HardDelete(TEntity entity)
        {
            base.HardDelete(entity);
        }
    }
}
