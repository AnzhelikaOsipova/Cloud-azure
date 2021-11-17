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
    public class AttendanceController: ControllerBase
    {
        private IAttendanceService _attendanceService;
        private IMapper _mapper;

        public AttendanceController(IAttendanceService attendanceService, IMapper mapper)
        {
            _attendanceService = attendanceService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/attendance/{id}")]
        public Attendance Get(int id)
        {
            _attendanceService.TryGet(out Models.Domain.Attendance[] attendance, id);
            return _mapper.Map<Models.Domain.Attendance, Attendance>(attendance.FirstOrDefault());
        }

        [HttpGet]
        [Route("/attendance/")]
        public IEnumerable<Attendance> Get()
        {
            _attendanceService.TryGet(out Models.Domain.Attendance[] attendance);
            return _mapper.Map<Models.Domain.Attendance[], Attendance[]>(attendance);
        }

        [HttpPut]
        [Route("/attendance/")]
        public IActionResult Create([FromBody] Attendance newAttendanceView)
        {
            var newAttendance = _mapper.Map<Attendance, Models.Domain.Attendance>(newAttendanceView);
            if (!_attendanceService.TryAdd(newAttendance))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost]
        [Route("/attendance/{id}")]
        public IActionResult Update(int id, [FromBody] Attendance updatedAttendanceView)
        {
            var updatedAttendance = _mapper.Map<Attendance, Models.Domain.Attendance>(updatedAttendanceView);
            if (!_attendanceService.TryUpdate(id, updatedAttendance))
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("/attendance/{id}")]
        public IActionResult Delete(int id)
        {
            if (!_attendanceService.TryDelete(id))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
