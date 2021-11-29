using Microsoft.SqlServer.Server;

namespace DapperLibrary.Utilities.Interfaces
{
    public interface ICrudUtility<T>
    {
        public Task<IEnumerable<T>> GetAsync(params KeyValuePair<string, object>[] parameters);
        public IEnumerable<T> Get(params KeyValuePair<string, object>[] parameters);
        public Task<T> GetSingleAsync(params KeyValuePair<string, object>[] parameters);
        public T GetSingle(params KeyValuePair<string, object>[] parameters);
        public int InsertSingleRecord(bool getReturnId = false, params KeyValuePair<string, object>[] parameters);
        public Task<int> InsertSingleRecordAsync(bool getReturnId = false, params KeyValuePair<string, object>[] parameters);
        public Task UpdateItemsWithTvpAsync(KeyValuePair<string, List<SqlDataRecord>> parameter);
        public void UpdateItemsWithTvp(KeyValuePair<string, List<SqlDataRecord>> parameter);
        public void UpdateSingleRecord(params KeyValuePair<string, object>[] parameters);
        public Task UpdateSingleRecordAsync(params KeyValuePair<string, object>[] parameters);
        public Task DeleteRecordsAsync((string paramName, IEnumerable<int> recordIdList) recordsIdentifier, params KeyValuePair<string, object>[] parameters);
        public void DeleteRecords((string paramName, IEnumerable<int> recordIdList) recordsIdentifier, params KeyValuePair<string, object>[] parameters);
    }
}
