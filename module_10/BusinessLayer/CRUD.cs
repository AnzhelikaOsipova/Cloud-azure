using AutoMapper;
using DataLayer;
using Models.Domain;

namespace BusinessLayer
{
    public class CRUD
    {
        IDataAccess _dataAccess;
        IMapper _mapper;

        public CRUD(IMapper mapper, IDataAccess dataAccess)
        {
            _mapper = mapper;
            _dataAccess = dataAccess;
        }

        public bool TryAdd<TSource, TDestination, TId>(TSource newItem)
            where TDestination: class, Models.IHasIdProperty<TId>
        {
            var newItemDB = _mapper.Map<TDestination>(newItem);
            return _dataAccess.TryAdd<TDestination, TId>(newItemDB);
        }

        public bool TryDelete<TDestination, TId>(TId id)
            where TDestination : class, Models.IHasIdProperty<TId>
        {
            return _dataAccess.TryDelete<TDestination, TId>(id);
        }

        public bool TryUpdate<TSource, TDestination, TId>(TId id, TSource updatedItem)
            where TDestination : class, Models.IHasIdProperty<TId>
        {
            var updatedItemDB = _mapper.Map<TDestination>(updatedItem);
            return _dataAccess.TryUpdate<TDestination, TId>(id, updatedItemDB);
        }

        public bool TryGetLectors(out Lector[] outLectors, int id = 0, string fio = null, string email = null)
        {
            bool success = _dataAccess.TryGetLectors(out Models.Database.Lector[] lectorsDB, id, fio, email);
            if (!success)
            {
                outLectors = null;
                return false;
            }
            outLectors = _mapper.Map<Lector[]>(lectorsDB);
            return true;
        }

        public bool TryGetStudents(out Student[] outStudents, int id = 0, string fio = null,
            string email = null, string phoneNumber = null)
        {
            bool success = _dataAccess.TryGetStudents(out Models.Database.Student[] studentsDB, id, fio, email, phoneNumber);
            if (!success)
            {
                outStudents = null;
                return false;
            }
            outStudents = _mapper.Map<Student[]>(studentsDB);
            return true;
        }

        public bool TryGetLections(out Lection[] outLections, int id = 0, string topic = null,
            string date = null, int lectorId = 0)
        {
            bool success = _dataAccess.TryGetLections(out Models.Database.Lection[] lectionsDB, id, topic, date, lectorId);
            if (!success)
            {
                outLections = null;
                return false;
            }
            outLections = _mapper.Map<Lection[]>(lectionsDB);
            return true;
        }

        public bool TryGetHomeworks(out Homework[] outHomeworks, int id = 0, int lectionId = 0,
            int studentId = 0, int mark = -1)
        {
            bool success = _dataAccess.TryGetHomeworks(out Models.Database.Homework[] homeworksDB, id, lectionId, studentId, mark);
            if (!success)
            {
                outHomeworks = null;
                return false;
            }
            outHomeworks = _mapper.Map<Homework[]>(homeworksDB);
            return true;
        }

        public bool TryGetAttendance(out Attendance[] outAttendance, int id = 0, int lectionId = 0,
            int studentId = 0)
        {
            bool success = _dataAccess.TryGetAttendance(out Models.Database.Attendance[] attendanceDB, id, lectionId, studentId);
            if (!success)
            {
                outAttendance = null;
                return false;
            }
            outAttendance = _mapper.Map<Attendance[]>(attendanceDB);
            return true;
        }
    }
}
