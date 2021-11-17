using Models.Domain;
using System.Collections.Generic;

namespace BusinessLayer.ReportFormats
{
    public interface IReportFormatConverter
    {
        public string Convert(List<Report> reports);
    }
}
