using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Microsoft.Data.SqlClient;

namespace Icon.EntityFrameworkCore.Matrix
{
    public interface ISharedSqlRepository<TEntity> : IRepository<TEntity, Guid>
    where TEntity : class, IEntity<Guid>
    {
        Task<IQueryable<TEntity>> GetSyncQueryable();
        Task<IQueryable<TEntity>> GetAllQueryNoQueryFilter();
        Task<List<TEntity>> GetAllEntitiesNoQueryFilter(CancellationToken cancellationToken = default);
        //Task BulkInsertOrUpdateAsync(List<TEntity> entities, List<string> propertiesToExcludeOnUpdate, CancellationToken cancellationToken = default);
        //Task BulkInsertOrUpdateStatusesAsync(List<TEntity> entities, CancellationToken cancellationToken = default);
        Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default);
        Task<int> ExecuteSqlRawWithParametersAsync(string sql, SqlParameter[] parameters, CancellationToken cancellationToken = default);
    }
    public class SharedSqlRepository<TEntity> : EfCoreRepositoryBase<IconDbContext, TEntity, Guid>, ISharedSqlRepository<TEntity>
           where TEntity : class, IEntity<Guid>
    {
        public SharedSqlRepository(IDbContextProvider<IconDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<IQueryable<TEntity>> GetSyncQueryable()
        {
            var dbContext = await GetDbContextAsync();
            return dbContext.Set<TEntity>().IgnoreQueryFilters().AsNoTracking().AsQueryable<TEntity>();
        }

        public virtual async Task<IQueryable<TEntity>> GetAllQueryNoQueryFilter()
        {
            var dbContext = await GetDbContextAsync();

            return dbContext.Set<TEntity>()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(entity => EF.Property<bool>(entity, "IsDeleted") == false);
        }

        public virtual async Task<List<TEntity>> GetAllEntitiesNoQueryFilter(CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return await dbContext.Set<TEntity>()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(entity => EF.Property<bool>(entity, "IsDeleted") == false)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<List<TEntity>> GetAllEntitiesNoQueryFilterByExternalIds(List<int> externalIds, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            // check if has IsDeleted property
            if (typeof(TEntity).GetProperty("IsDeleted") == null)
            {
                return await dbContext.Set<TEntity>()
                    .IgnoreQueryFilters()
                    .AsNoTracking()
                    .Where(entity => externalIds.Contains(EF.Property<int>(entity, "ExternalId")))
                    .ToListAsync(cancellationToken);
            }

            return await dbContext.Set<TEntity>()
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(entity => externalIds.Contains(EF.Property<int>(entity, "ExternalId")) && EF.Property<bool>(entity, "IsDeleted") == false)
                .ToListAsync(cancellationToken);
        }

        // public virtual async Task BulkInsertOrUpdateAsync(
        //     List<TEntity> entities, List<string> propertiesToExcludeOnUpdate, CancellationToken cancellationToken = default)
        // {
        //     if (entities.Count == 0)
        //     {
        //         return;
        //     }

        //     if (entities[0].GetType().GetProperty("CreationTime") != null)
        //     {
        //         foreach (var entity in entities)
        //         {
        //             entity.GetType().GetProperty("CreationTime")?.SetValue(entity, DateTime.Now);
        //         }
        //     }

        //     var dbContext = await GetDbContextAsync();
        //     var bulkConfig = new BulkConfig
        //     {
        //         UpdateByProperties = new List<string> { "ExternalId" },
        //         PropertiesToExcludeOnUpdate = propertiesToExcludeOnUpdate
        //     };

        //     if (typeof(TEntity) == typeof(Segment))
        //     {
        //         bulkConfig.UpdateByProperties = new List<string> { "ExternalTripId", "SequenceNumber" };
        //         bulkConfig.PropertiesToExcludeOnUpdate = propertiesToExcludeOnUpdate;
        //     }

        //     if (typeof(TEntity) == typeof(ClientBlockade))
        //     {
        //         bulkConfig.UpdateByProperties = new List<string> { "Id" };
        //         bulkConfig.PropertiesToExcludeOnUpdate = propertiesToExcludeOnUpdate;
        //     }

        //     if (typeof(TEntity) == typeof(LocationService))
        //     {
        //         bulkConfig.UpdateByProperties = new List<string> { "LocationId", "ServiceTypeId" };
        //         bulkConfig.PropertiesToExcludeOnUpdate = propertiesToExcludeOnUpdate;
        //     }

        //     if (typeof(TEntity) == typeof(LocationClientService))
        //     {
        //         bulkConfig.UpdateByProperties = new List<string> { "LocationId", "ClientId", "ServiceTypeId" };
        //         bulkConfig.PropertiesToExcludeOnUpdate = propertiesToExcludeOnUpdate;
        //     }

        //     await dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig, cancellationToken: cancellationToken);
        //     await dbContext.SaveChangesAsync(cancellationToken);
        // }

        // public async Task BulkDeleteEmptyRoutesAsync(CancellationToken cancellationToken = default)
        // {
        //     var dbContext = await GetDbContextAsync();
        //     var emptyRoutes = await dbContext.Set<Route>()
        //         .IgnoreQueryFilters()
        //         .Where(route => route.Trips.Count == 0)
        //         .ToListAsync(cancellationToken);

        //     await dbContext.BulkDeleteAsync(emptyRoutes, cancellationToken: cancellationToken);
        //     await dbContext.SaveChangesAsync(cancellationToken);
        // }

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

        // public virtual async Task BulkInsertOrUpdateStatusesAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        // {
        //     if (entities.Count == 0)
        //     {
        //         return;
        //     }

        //     var dbContext = await GetDbContextAsync();
        //     var bulkConfig = new BulkConfig
        //     {
        //         UpdateByProperties = new List<string> { "Id" },
        //     };

        //     if (typeof(TEntity) == typeof(Segment))
        //     {
        //         bulkConfig.PropertiesToIncludeOnUpdate = new List<string> { "TransitStatusId" };
        //     }

        //     if (typeof(TEntity) == typeof(Trip))
        //     {
        //         bulkConfig.PropertiesToIncludeOnUpdate = new List<string> { "TransitStatusId", "SyncStatus" };
        //     }

        //     if (typeof(TEntity) == typeof(Route))
        //     {
        //         bulkConfig.PropertiesToIncludeOnUpdate = new List<string> { "TransitStatusId", "SyncStatus" };
        //     }

        //     await dbContext.BulkInsertOrUpdateAsync(entities, bulkConfig, cancellationToken: cancellationToken);
        //     await dbContext.SaveChangesAsync(cancellationToken);
        // }



    }
}