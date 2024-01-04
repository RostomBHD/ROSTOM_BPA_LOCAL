using System;
using System.Data;
using System.IO;

public class SqlRecordsetToXmlConverter
{
    public string ConvertToXml(DataTable dataTable)
    {
        if (dataTable == null)
        {
            throw new ArgumentNullException(nameof(dataTable), "DataTable cannot be null.");
        }

        using (StringWriter sw = new StringWriter())
        {
            dataTable.WriteXml(sw);
            return sw.ToString();
        }
    }
}
