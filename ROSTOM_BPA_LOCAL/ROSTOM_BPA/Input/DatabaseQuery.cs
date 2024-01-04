using System.Data;
using System.Data.Odbc;

namespace ROSTOM_BPA_TOOLS.Input
{
    public class DatabaseQuery
    {
        public DataTable ExecuteQuery(string connectionString, string query)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                using (OdbcCommand cmd = new OdbcCommand(query, conn))
                {
                    conn.Open();
                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}
