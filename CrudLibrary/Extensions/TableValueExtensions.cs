using DapperLibrary.Models;
using System.ComponentModel;

namespace DapperLibrary.Extensions
{
    public static class TableValueExtensions
    {
        /// <summary>
        /// Converts to datatableTvp.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iList">The i list.</param>
        /// <param name="SqlTableTypeString">The SQL table type string.</param>
        /// <returns></returns>
        public static DataTableTvp ToDataTableTpv<T>(this List<T> iList, string SqlTableTypeString)
        {
            var idPropertyName = $"{typeof(T).Name}Id";

            var dataTableTvp = new DataTableTvp(SqlTableTypeString);
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));

            bool isComplexType = (!(propertyDescriptorCollection is null) && (propertyDescriptorCollection.Count > 0));

            if (isComplexType)
            {
                for (int i = 0; i < propertyDescriptorCollection.Count; i++)
                {
                    PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                    Type type = propertyDescriptor.PropertyType;

                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = Nullable.GetUnderlyingType(type);
                    }

                    dataTableTvp.Columns.Add(propertyDescriptor.Name, type);
                }
            }
            else
            {
                dataTableTvp.Columns.Add(typeof(T).Name, typeof(T));
            }
            if (isComplexType)
            {
                object[] values = new object[propertyDescriptorCollection.Count];
                foreach (T iListItem in iList)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                    }
                    dataTableTvp.Rows.Add(values);
                }
            }
            else
            {
                foreach (T iListItem in iList)
                {
                    dataTableTvp.Rows.Add(iListItem);
                }
            }
            return dataTableTvp;
        }
    }
}
