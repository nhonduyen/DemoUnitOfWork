using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Recruiter.Infrastructure.Common;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using Context = Microsoft.EntityFrameworkCore.DbContext;
using System.Reflection;

namespace Recruiter.Infrastructure.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static async Task<List<TEntity>> GetLargeWhereInSqlTempTable<TEntity>(this DbContext dbContext, List<Guid> listWhereInIds, Func<bool, List<TEntity>> func, int maxWhereIn = 300)
        {
            var result = new List<TEntity>();
            if (listWhereInIds == null || !listWhereInIds.Any()) return await Task.FromResult(result);

            var useTemptable = false;
            if (listWhereInIds.Count >= maxWhereIn)
            {
                useTemptable = true;
                await dbContext.CreateTempTableAsync($"#{nameof(TempTableData)}", listWhereInIds);
                result = func.Invoke(useTemptable);
                await dbContext.DeleteTempTableAsync($"#{nameof(TempTableData)}");
            }
            else
            {
                result = func.Invoke(useTemptable);
            }

            return await Task.FromResult(result);
        }
        public static async Task<bool> CreateTempTableAsync(this DbContext dbContext, string tableTemp, List<Guid> listWhereInIds)
        {
            bool result = false;
            if (listWhereInIds == null || !listWhereInIds.Any()) return await Task.FromResult(result);

            var simpleLookups = new List<TempTableData>(listWhereInIds.Distinct().Select(x => new TempTableData { Id = x }));
            var columns = new List<string> { nameof(TempTableData.Id) };
            try
            {
                var sqlConnection = (SqlConnection)dbContext.Database.GetDbConnection();
                var sqlCommand = $@"IF OBJECT_ID(N'tempdb..{tableTemp}') IS NOT NULL DROP TABLE {tableTemp};
                                    CREATE TABLE {tableTemp} (id uniqueidentifier, PRIMARY KEY(id));";
                await dbContext.Database.OpenConnectionAsync();
                await dbContext.Database.ExecuteSqlRawAsync(sqlCommand);
                using (var bulkcopy = new SqlBulkCopy(sqlConnection))
                {
                    bulkcopy.DestinationTableName = tableTemp;
                    using (var dataReader = new ObjectDataReader<TempTableData>(simpleLookups))
                    {
                        await bulkcopy.WriteToServerAsync(dataReader);
                        bulkcopy.Close();
                    }
                    result = true;
                }
            }
            catch(Exception ex)
            {
                await dbContext.Database.CloseConnectionAsync();
                throw;
            }
            
            return result;
        }

        public static async Task<bool> DeleteTempTableAsync(this DbContext dbContext, string tableTemp)
        {
            bool result = false;

            using (var sqlConnection = (SqlConnection)dbContext.Database.GetDbConnection())
            {
                var sqlCommand = $@"IF OBJECT_ID(N'tempdb..{tableTemp}') IS NOT NULL DROP TABLE {tableTemp};";
                var rowEffected = await dbContext.Database.ExecuteSqlRawAsync(sqlCommand);
                result = rowEffected > 0;
            }
            return result;
        }

        public static async Task<List<TEntity>> AsPageOrderedListAsync<TEntity>(this IQueryable<TEntity> query, bool isOrderedById = false, int count = -1, int pageSize = 10000)
        {
            List<TEntity> result = new List<TEntity>();

            if (count == -1)
            {
                count = await query.CountAsync();
            }
            var totalPage = Math.Ceiling((decimal)count / pageSize);

            // In case the query has ordered by != Id or no paging
            if (!isOrderedById || totalPage <= 1)
            {
                for (int page = 0; page < totalPage; page++)
                {
                    var items = await query.Skip(page * pageSize).Take(pageSize).ToListAsync();
                    result.AddRange(items);
                }

                return result;
            }

            // In case the query has ordered by Id and paging
            object lastIdValue = null;
            bool isGuidId = typeof(IGuidKeyEntity).GetTypeInfo().IsAssignableFrom(typeof(TEntity).Ge‌​tTypeInfo());
            bool isIntId = typeof(IIntKeyEntity).GetTypeInfo().IsAssignableFrom(typeof(TEntity).Ge‌​tTypeInfo());

            for (int page = 0; page < totalPage; page++)
            {
                List<TEntity> items = new List<TEntity>();
                if (lastIdValue != null && isGuidId)
                {
                    items = await query.Where(x => ((IGuidKeyEntity)x).Id.CompareTo((Guid)lastIdValue) > 0).Take(pageSize).ToListAsync();
                }
                else if (lastIdValue != null && isIntId)
                {
                    items = await query.Where(x => ((IIntKeyEntity)x).Id > (int)lastIdValue).Take(pageSize).ToListAsync();
                }
                else
                {
                    items = await query.Skip(page * pageSize).Take(pageSize).ToListAsync();
                }

                if ((page < totalPage - 1) && (isGuidId || isIntId))
                {
                    var lastItem = items.LastOrDefault();
                    if (lastItem != null)
                    {
                        lastIdValue = typeof(TEntity).GetProperty("Id")?.GetValue(lastItem, null);
                    }
                    else
                        lastIdValue = null;
                }

                result.AddRange(items);
            }

            return result;
        }

        public static async Task<List<TEntity>> WhereBulkContainsAsync<TEntity>(this Context dbContext, List<Guid> listWhereInIds, IQueryable<Guid> tempTable, string keyContains, Expression<Func<TEntity, TEntity>> selector = null, Expression<Func<TEntity, object>> orderBy = null, bool isTracking = false, int batchRead = 10000, Expression<Func<TEntity, bool>> extraCondition = null, params Expression<Func<TEntity, Object>>[] includes)
            where TEntity : class
        {
            const int MinimumIdsForCreateTempTable = 300;
            var result = new List<TEntity>();

            if (listWhereInIds.Count == 0)
                return await Task.FromResult(result);

            var query = isTracking ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var useTempTable = listWhereInIds.Count > MinimumIdsForCreateTempTable;
            var collectionIds = useTempTable ? tempTable : listWhereInIds.AsEnumerable();

            query = query.Where(p => collectionIds.Contains(EF.Property<Guid>(p, keyContains)));

            if (extraCondition != null)
            {
                query = query.Where(extraCondition);
            }

            if (useTempTable)
            {
                await dbContext.CreateTempTableAsync($"#{nameof(Common.TempTableData)}", listWhereInIds);
            }

            var count = await query.CountAsync();
            var isOrderedById = false;
            if (orderBy != null)
            {
                query = query.OrderBy(orderBy);
            }
            else if (count > batchRead)
            {
                query = query.OrderBy(p => EF.Property<object>(p, "Id"));
                isOrderedById = true;
            }

            if (selector != null)
            {
                query = query.Select(selector);
            }

            result = await query.AsPageOrderedListAsync(isOrderedById, count, batchRead);
            if (useTempTable)
            {
                await dbContext.DeleteTempTableAsync($"#{nameof(Common.TempTableData)}");
            }
            return result;
        }
    }
}
