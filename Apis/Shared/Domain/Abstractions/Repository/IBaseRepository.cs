using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Domain.Abstractions.Enumerations;
using Shared.Domain.Abstractions.Primitives;

namespace Shared.Domain.Abstractions.Repository
{
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> InsertAsync(
            TEntity entity,
            bool isBatchOperation = true,
            CancellationToken cancellationToken = default
        );

        Task DeleteAsync(
            TEntity entity,
            bool isBatchOperation = true,
            CancellationToken cancellationToken = default
        );

        Task UpdateAsync(
            TEntity entity,
            bool isBatchOperation = true,
            CancellationToken cancellationToken = default
        );

        Task<List<TEntity>> BulkInsertOrUpdateAsync(
            List<TEntity> entities,
            CancellationToken cancellationToken = default
        );

        Task<List<TEntity>> BulkDeleteAsync(
            List<TEntity> entities,
            CancellationToken cancellationToken = default
        );

        Task<Pagination<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int offset = 0,
            int limit = 5,
            SortBy? sortBy = SortBy.Id,
            bool sortOrderAscending = false
        );

        Task<Pagination<TEntity>> Search(
            string search,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int offset = 0,
            int limit = 5,
            SortBy? sortBy = SortBy.Id,
            bool sortOrderAscending = false
        );
    }
}
