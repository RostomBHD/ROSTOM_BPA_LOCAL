using System;
using System.Data;
using System.Data.SqlClient; // Use the SqlConnection and SqlCommand classes for ADO.NET

namespace ROSTOM_BPA_TOOLS.Input
{
    public class DatabaseQuery
    {
        public DataTable ExecuteQuery(string connectionString, string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}
