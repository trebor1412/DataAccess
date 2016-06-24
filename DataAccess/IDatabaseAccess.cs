using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess
{
    public interface IDatabaseAccess
    {
        string ConnectionString { get; }
        /// <summary>
        /// 關閉連線
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// 開啟連線
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// 查詢資料並以DataTable回傳
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        DataTable QueryWithDataTable(DataAccessCommand command);

        /// <summary>
        /// 查詢資料並以IDataReader回傳
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        IDataReader QueryWithDataReader(DataAccessCommand command);

        /// <summary>
        /// 執行指令並回傳資料影響數
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        int ExecuteCommand(DataAccessCommand command);

        /// <summary>
        /// 執行交易並回傳資料影響數
        /// </summary>
        /// <param name="commandCollection"></param>
        /// <returns></returns>
        int ExecuteTransaction(IEnumerable<DataAccessCommand> commandCollection);
    }
}
