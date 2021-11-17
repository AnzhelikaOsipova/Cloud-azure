using Models.Domain;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BusinessLayer.ReportFormats
{
    public class ReportXMLConverter : IReportFormatConverter
    {
        public string Convert(List<Report> reports)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Report>));
            using (StringWriter sw = new StringWriter())
            {
                formatter.Serialize(sw, reports);
                return sw.ToString();
            }            
        }
    }
}
