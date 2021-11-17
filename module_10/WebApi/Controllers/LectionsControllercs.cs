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
    public class LectionsController : ControllerBase
    {
        private ILectionsService _lectionsService;
        private IMapper _mapper;

        public LectionsController(ILectionsService lectionsService, IMapper mapper)
        {
            _lectionsService = lectionsService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/lection/{id}")]
        public Lection Get(int id)
        {
            _lectionsService.TryGet(out Models.Domain.Lection[] Lection, id);
            return _mapper.Map<Models.Domain.Lection, Lection>(Lection.FirstOrDefault());
        }

        [HttpGet]
        [Route("/lection/")]
        public IEnumerable<Lection> Get()
        {
            _lectionsService.TryGet(out Models.Domain.Lection[] Lection);
            return _mapper.Map<Models.Domain.Lection[], Lection[]>(Lection);
        }

        [HttpPut]
        [Route("/lection/")]
        public IActionResult Create([FromBody] Lection newLectionView)
        {
            var newLection = _mapper.Map<Lection, Models.Domain.Lection>(newLectionView);
            if (!_lectionsService.TryAdd(newLection))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Route("/lection/{id}")]
        public IActionResult Update(int id, [FromBody] Lection updatedLectionView)
        {
            var updatedLection = _mapper.Map<Lection, Models.Domain.Lection>(updatedLectionView);
            if (!_lectionsService.TryUpdate(id, updatedLection))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("/lection/{id}")]
        public IActionResult Delete(int id)
        {
            if (!_lectionsService.TryDelete(id))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}

