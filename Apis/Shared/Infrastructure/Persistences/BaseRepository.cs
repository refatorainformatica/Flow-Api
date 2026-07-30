//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading;
//using System.Threading.Tasks;
//using EFCore.BulkExtensions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Query;
//using Shared.Domain.Abstractions.Enumerations;
//using Shared.Domain.Abstractions.Primitives;
//using Shared.Domain.Abstractions.Repository;
//using Shared.Infrastructure.Extensions;

//namespace Shared.Infrastructure.Persistences
//{
//    public class BaseRepository<TEntity>(DbContext context) : IBaseRepository<TEntity>
//        where TEntity : BaseEntity
//    {
//        protected readonly DbContext dbContext = context;

//        public async Task<List<TEntity>> BulkDeleteAsync(
//            List<TEntity> entities,
//            CancellationToken cancellationToken = default
//        )
//        {
//            var bulkConfig = new BulkConfig { BatchSize = 4000, BulkCopyTimeout = 0 };

//            await dbContext.Database.BeginTransactionAsync(cancellationToken);
//            await dbContext.BulkDeleteAsync(
//                entities,
//                bulkConfig,
//                cancellationToken: cancellationToken
//            );
//            await dbContext.Database.CommitTransactionAsync(cancellationToken);

//            foreach (var entity in entities)
//            {
//                entity.OnRemovedEvent();
//            }

//            return entities;
//        }

//        public async Task<List<TEntity>> BulkInsertOrUpdateAsync(
//            List<TEntity> entities,
//            CancellationToken cancellationToken = default
//        )
//        {
//            var bulkConfig = new BulkConfig
//            {
//                BatchSize = 4000,
//                BulkCopyTimeout = 0,
//                IncludeGraph = true,
//                PreserveInsertOrder = true,
//            };

//            await dbContext.Database.BeginTransactionAsync(cancellationToken);
//            await dbContext.BulkInsertOrUpdateAsync(
//                entities,
//                bulkConfig,
//                cancellationToken: cancellationToken
//            );
//            await dbContext.Database.CommitTransactionAsync(cancellationToken);

//            return entities;
//        }

//        public async Task DeleteAsync(
//            TEntity entity,
//            bool isBatchOperation = true,
//            CancellationToken cancellationToken = default
//        )
//        {
//            if (isBatchOperation)
//            {
//                await BulkDeleteAsync([entity], cancellationToken);
//            }
//            else
//            {
//                await dbContext.Database.BeginTransactionAsync(cancellationToken);
//                TEntity entityToDelete = await dbContext.Set<TEntity>().FindAsync(entity.Id);

//                if (dbContext.Entry(entityToDelete).State == EntityState.Detached)
//                {
//                    dbContext.Set<TEntity>().Attach(entityToDelete);
//                }

//                dbContext.Set<TEntity>().Remove(entityToDelete);
//                await dbContext.SaveChangesAsync(cancellationToken);
//                await dbContext.Database.CommitTransactionAsync(cancellationToken);
//            }

//            entity.OnRemovedEvent();
//        }

//        public async Task<TEntity> InsertAsync(
//            TEntity entity,
//            bool isBatchOperation = true,
//            CancellationToken cancellationToken = default
//        )
//        {
//            if (isBatchOperation)
//            {
//                await BulkInsertOrUpdateAsync([entity], cancellationToken);
//            }
//            else
//            {
//                await dbContext.Database.BeginTransactionAsync(cancellationToken);
//                dbContext.Set<TEntity>().Add(entity);
//                await dbContext.SaveChangesAsync(cancellationToken);
//                await dbContext.Database.CommitTransactionAsync(cancellationToken);
//            }

//            entity.OnCreatedEvent();

//            return await Task.FromResult(entity);
//        }

//        public async Task UpdateAsync(
//            TEntity entity,
//            bool isBatchOperation = true,
//            CancellationToken cancellationToken = default
//        )
//        {
//            if (isBatchOperation)
//            {
//                await BulkInsertOrUpdateAsync([entity], cancellationToken);
//            }
//            else
//            {
//                await dbContext.Database.BeginTransactionAsync(cancellationToken);
//                dbContext.Set<TEntity>().Update(entity);
//                await dbContext.SaveChangesAsync(cancellationToken);
//                await dbContext.Database.CommitTransactionAsync(cancellationToken);
//            }

//            entity.OnEditedEvent();
//        }

//        public async Task<Pagination<TEntity>> GetAsync(
//            Expression<Func<TEntity, bool>> filter = null,
//            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//            int offset = 0,
//            int limit = 5,
//            SortBy? sortBy = SortBy.Id,
//            bool sortOrderAscending = false
//        )
//        {
//            IQueryable<TEntity> query = dbContext.Set<TEntity>();

//            sortBy ??= SortBy.Id;

//            if (include != null)
//            {
//                query = include(query);
//            }

//            if (filter != null)
//            {
//                query = query.Where(filter).SortBy(sortBy.ToString(), sortOrderAscending);
//            }

//            return await Task.FromResult(
//                query
//                    .Where(x => x.DeletedAt == null)
//                    .SortBy(sortBy.ToString(), sortOrderAscending)
//                    .CreatePagination(offset, limit) ?? new Pagination<TEntity>()
//            );
//        }

//        public async Task<Pagination<TEntity>> Search(
//            string search,
//            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
//            int offset = 0,
//            int limit = 5,
//            SortBy? sortBy = SortBy.Id,
//            bool sortOrderAscending = false
//        )
//        {
//            IQueryable<TEntity> query = dbContext.Set<TEntity>();

//            sortBy ??= SortBy.Id;

//            if (include != null)
//            {
//                query = include(query);
//            }

//            query = query.SearchBy(search).SortBy(sortBy.ToString(), sortOrderAscending);

//            return await Task.FromResult(
//                query.Where(x => x.DeletedAt == null).CreatePagination(offset, limit)
//                    ?? new Pagination<TEntity>()
//            );
//        }
//    }
//}
