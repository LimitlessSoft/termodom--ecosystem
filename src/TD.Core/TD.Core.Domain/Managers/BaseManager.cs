using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Core.Contracts;
using TD.Core.Contracts.Enums.ValidationCodes;
using TD.Core.Contracts.Extensions;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Http.Interfaces;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Interfaces;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Validators;

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
			if (!httpContext.User.Identity.IsAuthenticated)
				return;

			var claims = httpContext.User?.Claims.ToList();
			if (claims == null)
				return;

			CurrentUser = new ContextUser();
			CurrentUser.Username =
				claims
					.FirstOrDefault(x => x.Type == Contracts.Constants.ClaimNames.CustomUsername)
					?.Value.ToString() ?? "UNDEFINED";
			CurrentUser.Id = Convert.ToInt32(
				claims
					.FirstOrDefault(x => x.Type == Contracts.Constants.ClaimNames.CustomUserId)
					?.Value
			);
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
				return Response<TEntity>.InternalServerError(
					CommonValidationCodes.COMM_005.GetDescription(String.Empty)
				);

			var response = new Response<TEntity>();
			if (request.IsRequestInvalid(response))
				return response;

			var entityMapper = (IMap<TEntity, TRequest>?)
				Constants.Container?.TryGetInstance(typeof(IMap<TEntity, TRequest>));

			var entity = new TEntity();
			if (!request.Id.HasValue)
			{
				var lastId = _dbContext
					.Set<TEntity>()
					.AsQueryable()
					.OrderByDescending(x => x.Id)
					.Select(x => x.Id)
					.FirstOrDefault();

				if (entityMapper == null)
					entity.InjectFrom(request);
				else
					entityMapper.Map(entity, request);

				entity.Id = ++lastId;
				entity.CreatedAt = DateTime.UtcNow;
				entity.CreatedBy = CurrentUser?.Id ?? 0;

				_dbContext.Set<TEntity>().Add(entity);
			}
			else
			{
				entity = _dbContext.Set<TEntity>().FirstOrDefault(x => x.Id == request.Id);

				if (entity == null)
					return Response<TEntity>.NotFound();

				if (entityMapper == null)
					entity.InjectFrom(request);
				else
					entityMapper.Map(entity, request);

				entity.UpdatedAt = DateTime.UtcNow;
				entity.UpdatedBy = CurrentUser?.Id ?? 0;

				_dbContext.Set<TEntity>().Update(entity);
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
		public TEntity Update<TEntity>(TEntity entity)
			where TEntity : class
		{
			if (_dbContext == null)
				throw new ArgumentNullException(nameof(_dbContext));

			_dbContext.Set<TEntity>().Update(entity);

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
		public TEntity Insert<TEntity>(TEntity entity)
			where TEntity : class
		{
			if (_dbContext == null)
				throw new ArgumentNullException(nameof(_dbContext));

			_dbContext.Set<TEntity>().Add(entity);

			_dbContext.SaveChanges();

			return entity;
		}

		/// <summary>
		/// Gets T entity table as queryable
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IQueryable<T> Queryable<T>()
			where T : class
		{
			if (_dbContext == null)
				throw new ArgumentNullException(nameof(_dbContext));

			return _dbContext.Set<T>().AsQueryable();
		}

		/// <summary>
		/// Gets first T entity
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Response<T> First<T>(Expression<Func<T, bool>> predicate)
			where T : class
		{
			if (_dbContext == null)
				return Response<T>.InternalServerError(
					CommonValidationCodes.COMM_005.GetDescription(String.Empty)
				);

			var entity = Queryable<T>().FirstOrDefault(predicate);
			if (entity == null)
				return Response<T>.NotFound();

			return new Response<T>(entity);
		}

		public Response<TPayload> First<TEntity, TPayload>(
			Expression<Func<TEntity, bool>> predicate
		)
			where TEntity : class
		{
			var response = new Response<TPayload>();
			var entityResponse = First(predicate);

			response.Merge(entityResponse);
			if (response.NotOk)
				return response;

			response.Payload = entityResponse.Payload.ToDto<TPayload, TEntity>();
			return response;
		}

		/// <summary>
		/// Deletes record from the database
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		public Response HardDelete<TEntity>(TEntity entity)
			where TEntity : class
		{
			_dbContext.Set<TEntity>().Remove(entity);

			_dbContext.SaveChanges();

			return new Response();
		}

		/// <summary>
		/// Deletes record from the database
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		public Response HardDelete<TEntity>(int id)
			where TEntity : class, IEntityBase
		{
			var entity = _dbContext?.Set<TEntity>().FirstOrDefault(x => x.Id == id);

			if (entity == null)
				return Response.NotFound();

			return HardDelete(entity);
		}

		/// <summary>
		/// Updates records is_active to false
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		public void SoftDelete<TEntity>(TEntity entity)
			where TEntity : class, IEntity
		{
			entity.IsActive = false;
			Update(entity);
		}
	}

	public class BaseManager<TManager, TEntity> : BaseManager<TManager>
		where TEntity : class, IEntity, new()
	{
		private readonly ILogger<TManager> _logger;
		private readonly DbContext _dbContext;

		public BaseManager(ILogger<TManager> logger, DbContext dbContext)
			: base(logger, dbContext)
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
			where TRequest : SaveRequest => Save<TEntity, TRequest>(request);

		/// <summary>
		/// Adds or save entity to database
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public Response<TPayload> Save<TRequest, TPayload>(
			TRequest request,
			Func<TEntity, Response<TPayload>> responseMapper
		)
			where TRequest : SaveRequest
		{
			var response = new Response<TPayload>();

			var saveResponse = base.Save<TEntity, TRequest>(request);
			response.Merge(saveResponse);
			if (response.NotOk)
				return response;

			return responseMapper(saveResponse.Payload);
		}

		/// <summary>
		/// Gets manager entity table as queryable
		/// </summary>
		/// <returns></returns>
		public IQueryable<TEntity> Queryable() => Queryable<TEntity>();

		/// <summary>
		/// Gets manager entity table as queryable
		/// </summary>
		/// <returns></returns>
		public IQueryable<TEntity> Queryable(Expression<Func<TEntity, bool>> predicate) =>
			Queryable<TEntity>().Where(predicate);

		/// <summary>
		/// Gets first manager entity
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Response<TEntity> First(Expression<Func<TEntity, bool>> predicate) =>
			base.First<TEntity>(predicate);

		/// <summary>
		/// Deletes record from the database
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		public Response HardDelete(TEntity entity) => base.HardDelete(entity);

		/// <summary>
		/// Deletes record from the database
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		public Response HardDelete(int id) => base.HardDelete<TEntity>(id);
	}
}
