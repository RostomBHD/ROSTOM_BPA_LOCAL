using System.Data;
using System.Data.Odbc;


namespace ROSTOM_BPA_TOOLS.Input
{
    public class CallStoredProcedure
    {
        public DataTable ExecuteStoredProcedure(string connectionString, string procedureName, List<OdbcParameter> parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                using (OdbcCommand cmd = new OdbcCommand(procedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    parameters.ForEach(param => cmd.Parameters.Add(param));

                    OdbcDataAdapter adapter = new OdbcDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }
    }
}
