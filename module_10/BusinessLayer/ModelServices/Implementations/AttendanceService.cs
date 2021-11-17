using AutoMapper;
using DataLayer;
using Microsoft.Extensions.Logging;
using Models.Domain;
using BusinessLayer.ModelServices.Contracts;

namespace BusinessLayer.ModelServices.Implementations
{
    public class AttendanceService: BaseService<Attendance, Models.Database.Attendance, int>, IAttendanceService
    {
        private ILogger _logger;
        public AttendanceService(IMapper mapper, IDataAccess dataAccess, ILogger logger):
            base(mapper, dataAccess)
        {
            _logger = logger;
        }

        protected override bool IsValid(Attendance item)
        {
            if (item.Id < 0)
            {
                _logger.LogWarning("Invalid attendance parameters: id mustn't be < 0, but was " + item.Id);
                return false;
            }
            if (item.LectionId < 1)
            {
                _logger.LogWarning("Invalid attendance parameters: lectionId mustn't be < 1, but was " + item.LectionId);
                return false;
            }
            if (item.StudentId < 1)
            {
                _logger.LogWarning("Invalid attendance parameters: studentId mustn't be < 1, but was " + item.StudentId);
                return false;
            }
            return true;
        }

        protected override bool IsValid(int id)
        {
            if (id < 1)
            {
                _logger.LogWarning("Invalid attendance parameters: id mustn't be < 1, but was " + id);
                return false;
            }
            return true;
        }

        public bool TryGet(out Attendance[] outAttendances, int id = 0, int lectionId = 0,
            int studentId = 0)
        {
            CRUD crud = new CRUD(_mapper, _dataAccess);
            return crud.TryGetAttendance(out outAttendances, id, lectionId, studentId);
        }
    }
}
