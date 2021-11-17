using Microsoft.AspNetCore.Mvc;
using BusinessLayer.ModelServices.Contracts;
using WebApi.ViewModels;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private IStudentsService _StudentsService;
        private IMapper _mapper;

        public StudentsController(IStudentsService studentsService, IMapper mapper)
        {
            _StudentsService = studentsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/student/{id}")]
        public Student Get(int id)
        {
            _StudentsService.TryGet(out Models.Domain.Student[] Student, id);
            return _mapper.Map<Models.Domain.Student, Student>(Student.FirstOrDefault());
        }

        [HttpGet]
        [Route("/student/")]
        public IEnumerable<Student> Get()
        {
            _StudentsService.TryGet(out Models.Domain.Student[] Student);
            return _mapper.Map<Models.Domain.Student[], Student[]>(Student);
        }

        [HttpPut]
        [Route("/student/")]
        public IActionResult Create([FromBody] Student newStudentView)
        {
            var newStudent = _mapper.Map<Student, Models.Domain.Student>(newStudentView);
            if (!_StudentsService.TryAdd(newStudent))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Route("/student/{id}")]
        public IActionResult Update(int id, [FromBody] Student updatedStudentView)
        {
            var updatedStudent = _mapper.Map<Student, Models.Domain.Student>(updatedStudentView);
            if (!_StudentsService.TryUpdate(id, updatedStudent))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("/student/{id}")]
        public IActionResult Delete(int id)
        {
            if (!_StudentsService.TryDelete(id))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
