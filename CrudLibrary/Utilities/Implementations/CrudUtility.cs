using Dapper;
using Dapper.TableValuedParameter;
using DapperLibrary.Extensions;
using DapperLibrary.Models;
using DapperLibrary.Utilities.Interfaces;
using Microsoft.SqlServer.Server;
using System.Data;
using System.Data.SqlClient;

namespace DapperLibrary.Utilities.Implementations
{
    public class CrudUtility<T> : ICrudUtility<T>
    {
        #region Private Variables
        private readonly string connectionString;
        private readonly int commandTimeoutSecs = 600;
        #endregion
        #region Constructors
        public CrudUtility(string connectionString) => this.connectionString = connectionString;
        #endregion
        #region Public Methods
        /// <summary>
        /// Gets the specified parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAsync(params KeyValuePair<string, object>[] parameters)
        {
            string storedProcedureName = $"usp_Get{typeof(T).Name}List";
            var dynamicParams = new DynamicParameters();

            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<T> returnedList = await db.QueryAsync<T>(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeoutSecs);
                return returnedList;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public IEnumerable<T> Get(params KeyValuePair<string, object>[] parameters)
        {
            string storedProcedureName = $"usp_Get{typeof(T).Name}List";
            var dynamicParams = new DynamicParameters();

            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                IEnumerable<T> returnedList = db.Query<T>(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeoutSecs);
                return returnedList;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }


        /// <summary>
        /// Gets the single.
        /// If more than one item is returned,
        /// a single empty new object is returned
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<T> GetSingleAsync(params KeyValuePair<string, object>[] parameters)
        {
            string storedProcedureName = $"usp_Get{typeof(T).Name}";
            var dynamicParams = new DynamicParameters();

            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                T returnedItem = await db.QuerySingleOrDefaultAsync<T>(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeoutSecs);
                return returnedItem;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Gets the single.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public T GetSingle(params KeyValuePair<string, object>[] parameters)
        {
            string storedProcedureName = $"usp_Get{typeof(T).Name}";
            var dynamicParams = new DynamicParameters();

            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                T returnedItem = db.QuerySingleOrDefault<T>(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, commandTimeout: commandTimeoutSecs);
                return returnedItem;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }


        /// <summary>
        /// Sets the itemswith TVP.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">The parameter.</param>
        public async Task UpdateItemsWithTvpAsync(KeyValuePair<string, List<SqlDataRecord>> parameter)
        {

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                string storedProcedureName = $"usp_Update{typeof(T).Name}List";
                IEnumerable<List<SqlDataRecord>> result = await db.QueryAsync<List<SqlDataRecord>>(storedProcedureName,
                    new Tvp($"@{parameter.Key}", typeof(T).Name, parameter.Value),
                     commandType: CommandType.StoredProcedure, commandTimeout: commandTimeoutSecs);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }
        /// <summary>
        /// Sets the itemswith TVP.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void UpdateItemsWithTvp(KeyValuePair<string, List<SqlDataRecord>> parameter)
        {

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                string storedProcedureName = $"usp_Update{typeof(T).Name}List";
                IEnumerable<List<SqlDataRecord>> result = db.Query<List<SqlDataRecord>>(storedProcedureName,
                    new Tvp($"@{parameter.Key}", typeof(T).Name, parameter.Value),
                     commandType: CommandType.StoredProcedure, commandTimeout: commandTimeoutSecs);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }
        /// <summary>
        /// Sets the single record.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameter">The parameter.</param>
        public async Task UpdateSingleRecordAsync(params KeyValuePair<string, object>[] parameters)
        {
            var dynamicParams = new DynamicParameters();
            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                string storedProcedureName = $"usp_Update{typeof(T).Name}";
                using IDbTransaction tran = db.BeginTransaction();
                try
                {
                    await db.ExecuteAsync(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: commandTimeoutSecs);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Sets the single record.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void UpdateSingleRecord(params KeyValuePair<string, object>[] parameters)
        {
            var dynamicParams = new DynamicParameters();
            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }

            using IDbConnection db = new SqlConnection(connectionString);

            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                string storedProcedureName = $"usp_Update{typeof(T).Name}";
                using IDbTransaction tran = db.BeginTransaction();
                try
                {
                    db.Execute(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: commandTimeoutSecs);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }

        }

        /// <summary>
        /// Inserts the single record.
        /// </summary>
        /// <param name="getReturnId">if set to <c>true</c> [get return identifier].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int InsertSingleRecord(bool getReturnId = false, params KeyValuePair<string, object>[] parameters)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                string storedProcedureName = $"usp_Create{typeof(T).Name}";
                var dynamicParams = new DynamicParameters();

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    dynamicParams.Add("@" + param.Key, param.Value);
                }
                if (getReturnId)
                {
                    dynamicParams.Add("@Id", null, DbType.Int32, ParameterDirection.Output, null);
                }
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }

                using IDbTransaction tran = db.BeginTransaction();
                try
                {
                    db.Execute(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: commandTimeoutSecs);
                    tran.Commit();
                    return getReturnId ? dynamicParams.Get<int>("@Id") : 1;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Inserts the single record asynchronous.
        /// </summary>
        /// <param name="getReturnId">if set to <c>true</c> [get return identifier].</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public async Task<int> InsertSingleRecordAsync(bool getReturnId = false, params KeyValuePair<string, object>[] parameters)
        {
            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                string storedProcedureName = $"usp_Create{typeof(T).Name}";
                var dynamicParams = new DynamicParameters();

                foreach (KeyValuePair<string, object> param in parameters)
                {
                    dynamicParams.Add("@" + param.Key, param.Value);
                }
                if (getReturnId)
                {
                    dynamicParams.Add("@Id", null, DbType.Int32, ParameterDirection.Output, null);
                }
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }

                using IDbTransaction tran = db.BeginTransaction();
                try
                {
                    await db.ExecuteAsync(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: commandTimeoutSecs);
                    tran.Commit();
                    return getReturnId ? dynamicParams.Get<int>("@Id") : 1;
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }



        /// <summary>
        /// Deletes the records.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="recordIdList">The record identifier list.</param>
        public async Task DeleteRecordsAsync((string paramName, IEnumerable<int> recordIdList) recordsIdentifier, params KeyValuePair<string, object>[] parameters)
        {
            DataTableTvp tvParam = recordsIdentifier.recordIdList.ToList().ToDataTableTpv("[dbo].[IntList]");

            var dynamicParams = new DynamicParameters();
            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }
            dynamicParams.Add("@" + recordsIdentifier.paramName, ((DataTableTvp)tvParam).AsTableValuedParameter(((DataTableTvp)tvParam).TableValueType));
            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                string storedProcedureName = $"usp_Delete{typeof(T).Name}List";
                using IDbTransaction tran = db.BeginTransaction();
                try
                {
                    await db.ExecuteAsync(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: commandTimeoutSecs);
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }
        }

        /// <summary>
        /// Deletes the records.
        /// </summary>
        /// <param name="paramName">Name of the parameter.</param>
        /// <param name="recordIdList">The record identifier list.</param>
        public void DeleteRecords((string paramName, IEnumerable<int> recordIdList) recordsIdentifier, params KeyValuePair<string, object>[] parameters)
        {
            DataTableTvp tvParam = recordsIdentifier.recordIdList.ToList().ToDataTableTpv("[dbo].[IntList]");

            var dynamicParams = new DynamicParameters();
            foreach (KeyValuePair<string, object> param in parameters)
            {
                dynamicParams.Add("@" + param.Key, param.Value);
            }

            dynamicParams.Add("@" + recordsIdentifier.paramName, ((DataTableTvp)tvParam).AsTableValuedParameter(((DataTableTvp)tvParam).TableValueType));

            using IDbConnection db = new SqlConnection(connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                {
                    db.Open();
                }
                string storedProcedureName = $"usp_Delete{typeof(T).Name}List";
                using IDbTransaction tran = db.BeginTransaction();
                try
                {
                    db.Execute(storedProcedureName, dynamicParams, commandType: CommandType.StoredProcedure, transaction: tran, commandTimeout: commandTimeoutSecs);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                {
                    db.Close();
                    db.Dispose();
                }
            }



        }
        #endregion
    }
}
