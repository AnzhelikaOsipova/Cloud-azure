using Microsoft.AspNetCore.Mvc;
using BusinessLayer.ModelServices.Contracts;
using WebApi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeworksController: ControllerBase
    {
        private IHomeworksService _homeworksService;
        private IMapper _mapper;

        public HomeworksController(IHomeworksService homeworksService, IMapper mapper)
        {
            _homeworksService = homeworksService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/homework/{id}")]
        public Homework Get(int id)
        {
            _homeworksService.TryGet(out Models.Domain.Homework[] Homework, id);
            return _mapper.Map<Models.Domain.Homework, Homework>(Homework.FirstOrDefault());
        }

        [HttpGet]
        [Route("/homework/")]
        public IEnumerable<Homework> Get()
        {
            _homeworksService.TryGet(out Models.Domain.Homework[] Homework);
            return _mapper.Map<Models.Domain.Homework[], Homework[]>(Homework);
        }

        [HttpPut]
        [Route("/homework/")]
        public IActionResult Create([FromBody] Homework newHomeworkView)
        {
            var newHomework = _mapper.Map<Homework, Models.Domain.Homework>(newHomeworkView);
            if (!_homeworksService.TryAdd(newHomework))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Route("/homework/{id}")]
        public IActionResult Update(int id, [FromBody] Homework updatedHomeworkView)
        {
            var updatedHomework = _mapper.Map<Homework, Models.Domain.Homework>(updatedHomeworkView);
            if (!_homeworksService.TryUpdate(id, updatedHomework))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("/homework/{id}")]
        public IActionResult Delete(int id)
        {
            if (!_homeworksService.TryDelete(id))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}

