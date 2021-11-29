using System.Data;

namespace DapperLibrary.Models
{
    public class DataTableTvp : DataTable
    {
        #region Properties
        public string TableValueType { get; set; }
        #endregion
        #region Constructors
        public DataTableTvp(string valueType) => TableValueType = valueType;

        #endregion
    }
}

