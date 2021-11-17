using Models.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BusinessLayer.ReportFormats
{
    public class ReportJSONConverter : IReportFormatConverter
    {
        public string Convert(List<Report> reports)
        {
            return JsonConvert.SerializeObject(reports, Formatting.Indented);
        }
    }
}
