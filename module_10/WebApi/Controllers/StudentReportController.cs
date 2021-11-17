using BusinessLayer;
using BusinessLayer.ReportFormats;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentReportController: ControllerBase
    {
        StudentReportGenerator _reportGenerator;

        public StudentReportController(StudentReportGenerator reportGenerator)
        {
            _reportGenerator = reportGenerator;
        }

        [HttpGet]
        [Route("/studentreport/{studentFio}")]
        public string Get(string studentFio)
        {
            _reportGenerator.TryMakeReportAboutStudent(studentFio, new ReportJSONConverter(), out string report);
            return report;
        }
    }
}
