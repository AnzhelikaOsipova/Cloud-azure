using BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeworksAnalyzerController: ControllerBase
    {
        HomeworksAnalyzer _analyzer;

        public HomeworksAnalyzerController(HomeworksAnalyzer analyzer)
        {
            _analyzer = analyzer;
        }

        [HttpPut]
        [Route("/homeworksanalyzer/")]
        public IActionResult Analyze()
        {
            if (!_analyzer.TryCheckMeanMark())
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
