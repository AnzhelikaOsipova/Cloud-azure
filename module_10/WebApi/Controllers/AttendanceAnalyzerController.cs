using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttendanceAnalyzerController : ControllerBase
    {
        AttendanceAnalyzer _analyzer;

        public AttendanceAnalyzerController(AttendanceAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        [HttpPut]
        [Route("/attendanceanalyzer/")]
        public IActionResult Analyze()
        {
            if(!_analyzer.TryCheckAttendance())
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
