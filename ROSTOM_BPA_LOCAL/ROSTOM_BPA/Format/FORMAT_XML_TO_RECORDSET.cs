using System;
using System.Data;
using System.IO;
using System.Xml;

public class XmlToRecordsetConverter
{
    public DataSet ConvertXmlToDataSet(string xmlString)
    {
        DataSet dataSet = new DataSet();

        using (StringReader stringReader = new StringReader(xmlString))
        {
            dataSet.ReadXml(stringReader);
        }

        return dataSet;
    }
}
