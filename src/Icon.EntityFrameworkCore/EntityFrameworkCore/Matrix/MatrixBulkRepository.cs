using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;
using Abp.Domain.Repositories;
using Microsoft.Data.SqlClient;

namespace Icon.EntityFrameworkCore.Matrix
{
    public interface IMatrixBulkRepository<TEntity> : IRepository<TEntity, Guid>
    where TEntity : class, IEntity<Guid>
    {
        Task<IQueryable<TEntity>> GetEntityQueryable();
        Task<IQueryable<TEntity>> GetEntityQueryableNoQueryFilter();
        Task BulkInsertAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        Task BulkInsertOrUpdateExcludeAsync(List<TEntity> entities, List<string> propertiesToExcludeOnUpdate, CancellationToken cancellationToken = default);
        Task BulkInsertOrUpdateIncludeAsync(List<TEntity> entities, List<string> propertiesToIncludeOnUpdate, CancellationToken cancellationToken = default);
        Task BulkInsertOrUpdateIncludeByAsync(List<TEntity> entities, List<string> propertiesToIncludeOnUpdate, List<string> UpdateBy, CancellationToken cancellationToken = default);
        Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default);
        Task<int> ExecuteSqlRawWithParametersAsync(string sql, SqlParameter[] parameters, CancellationToken cancellationToken = default);
    }
    public class MatrixBulkRepository<TEntity> : EfCoreRepositoryBase<IconDbContext, TEntity, Guid>, IMatrixBulkRepository<TEntity>
           where TEntity : class, IEntity<Guid>
    {
        public MatrixBulkRepository(IDbContextProvider<IconDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<IQueryable<TEntity>> GetEntityQueryable()
        {
            var dbContext = await GetDbContextAsync();
            return dbContext.Set<TEntity>().IgnoreQueryFilters().AsNoTracking().AsQueryable<TEntity>();
        }

        public virtual async Task<IQueryable<TEntity>> GetEntityQueryableNoQueryFilter()
        {
            var dbContext = await GetDbContextAsync();
            return dbContext.Set<TEntity>()
                .IgnoreQueryFilters()
                .AsNoTracking();
        }

        public virtual async Task BulkInsertAsync(
            List<TEntity> entities, CancellationToken cancellationToken = default)
        {
            if (entities.Count == 0)
            {
                return;
            }

            var dbContext = await GetDbContextAsync();

            await dbContext.BulkInsertAsync(entities, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task BulkInsertOrUpdateExcludeAsync(
            List<TEntity> entities, List<string> propertiesToExcludeOnUpdate, CancellationToken cancellationToken = default)
        {
            if (entities.Count == 0)
            {
                return;
            }

            var dbContext = await GetDbContextAsync();
            var bulkConfig = new BulkConfig
            {
                UpdateByProperties = new List<string> { "Id" },
                PropertiesToExcludeOnUpdate = propertiesToExcludeOnUpdate
            };

            await dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task BulkInsertOrUpdateIncludeAsync(
            List<TEntity> entities, List<string> propertiesToIncludeOnUpdate, CancellationToken cancellationToken = default)
        {
            if (entities.Count == 0)
            {
                return;
            }

            var dbContext = await GetDbContextAsync();
            var bulkConfig = new BulkConfig
            {
                UpdateByProperties = new List<string> { "Id" },
                PropertiesToIncludeOnUpdate = propertiesToIncludeOnUpdate
            };

            await dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task BulkInsertOrUpdateIncludeByAsync(
            List<TEntity> entities, List<string> propertiesToIncludeOnUpdate, List<string> UpdateBy, CancellationToken cancellationToken = default)
        {
            if (entities.Count == 0)
            {
                return;
            }

            var dbContext = await GetDbContextAsync();
            var bulkConfig = new BulkConfig
            {
                UpdateByProperties = UpdateBy,
                PropertiesToIncludeOnUpdate = propertiesToIncludeOnUpdate
            };

            await dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var rows = await dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            return rows;
        }

        public async Task<int> ExecuteSqlRawWithParametersAsync(string sql, SqlParameter[] parameters, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var rows = await dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
            return rows;
        }
    }
}