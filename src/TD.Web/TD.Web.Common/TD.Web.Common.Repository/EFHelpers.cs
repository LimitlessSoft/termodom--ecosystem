using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace TD.Web.Common.Contracts.Helpers
{
    public static class EFHelpers
    {
        public static IIncludableQueryable<TEntity, TProperty> IncludeRecursively<TEntity, TProperty>(
            this IQueryable<TEntity> source,
            int depth,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            if (depth <= 1)
                return source.Include(navigationPropertyPath);

            return source.IncludeRecursively(depth - 1, navigationPropertyPath);
        }


        public static IIncludableQueryable<TEntity, TProperty> ThenIncludeRecursively<TEntity, TProperty>(
            this IIncludableQueryable<TEntity, IEnumerable<TProperty>> source,
            int depth,
            Expression<Func<TProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            if (depth <= 1)
                return source.ThenInclude(navigationPropertyPath);

            return source.ThenInclude(navigationPropertyPath).ThenIncludeRecursively(depth - 1, navigationPropertyPath);
        }

        public static IIncludableQueryable<TEntity, TProperty> ThenIncludeRecursively<TEntity, TProperty>(
            this IIncludableQueryable<TEntity, TProperty> source,
            int depth,
            Expression<Func<TProperty, TProperty>> navigationPropertyPath)
            where TEntity : class
        {
            if (depth <= 1)
                return source.ThenInclude(navigationPropertyPath);

            return source.ThenInclude(navigationPropertyPath).ThenIncludeRecursively(depth - 1, navigationPropertyPath);
        }
    }
}
