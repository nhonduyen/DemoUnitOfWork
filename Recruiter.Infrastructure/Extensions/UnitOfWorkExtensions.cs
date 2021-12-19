using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Recruiter.Infrastructure.Common;
using Microsoft.Data.SqlClient;

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
    }
}
