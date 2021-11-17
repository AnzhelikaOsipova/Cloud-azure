using BusinessLayer;
using BusinessLayer.ReportFormats;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LectionReportController: ControllerBase
    {
        LectionReportGenerator _reportGenerator;

        public LectionReportController(LectionReportGenerator reportGenerator)
        {
            _reportGenerator = reportGenerator;
        }

        [HttpGet]
        [Route("/lectionreport/{topic}")]
        public string Get(string topic)
        {
            _reportGenerator.TryMakeReportAboutLection(topic, new ReportJSONConverter(), out string report);
            return report;
        }
    }
}
