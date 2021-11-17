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
    public class LectorsController: ControllerBase
    {
        private ILectorsService _lectorsService;
        private IMapper _mapper;

        public LectorsController(ILectorsService lectorsService, IMapper mapper)
        {
            _lectorsService = lectorsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/lector/{id}")]
        public Lector Get(int id)
        {
            _lectorsService.TryGet(out Models.Domain.Lector[] lectors, id);
            return _mapper.Map<Models.Domain.Lector, Lector>(lectors.FirstOrDefault());
        }

        [HttpGet]
        [Route("/lector/")]
        public IEnumerable<Lector> Get()
        {
            _lectorsService.TryGet(out Models.Domain.Lector[] lectors);
            return _mapper.Map<Models.Domain.Lector[], Lector[]>(lectors);
        }

        [HttpPut]
        [Route("/lector/")]
        public IActionResult Create([FromBody] Lector newLectorView)
        {
            var newLector = _mapper.Map<Lector, Models.Domain.Lector>(newLectorView);
            if (!_lectorsService.TryAdd(newLector))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Route("/lector/{id}")]
        public IActionResult Update(int id, [FromBody] Lector updatedLectorView)
        {
            var updatedLector = _mapper.Map<Lector, Models.Domain.Lector>(updatedLectorView);
            if (!_lectorsService.TryUpdate(id, updatedLector))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("/lector/{id}")]
        public IActionResult Delete(int id)
        {
            if (!_lectorsService.TryDelete(id))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
