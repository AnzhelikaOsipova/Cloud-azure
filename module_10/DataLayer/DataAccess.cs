using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Database;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class DataAccess: IDataAccess
    {
        private readonly DbContextOptions _dbOptions;
        private ILogger _logger;

        public DataAccess(DbContextOptions options, ILogger logger)
        {
            _dbOptions = options;
            _logger = logger;
        }

        public void ReCreateDatabase()
        {
            using (EducationServerContext edContext = new EducationServerContext(_dbOptions))
            {
                try
                {
                    edContext.ReCreateDatabase();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public bool TryAdd<T, TId>(T itemToAdd) where T: class, IHasIdProperty<TId>
        {
            using (var crud = new BasicCRUD<T,TId>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryAdd(itemToAdd))
                {
                    return false;
                }
                return true;
            }
        }

        public bool TryDelete<T, TId>(TId id) where T : class, IHasIdProperty<TId>
        {
            using (var crud = new BasicCRUD<T,TId>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryDelete(id))
                {
                    return false;
                }
                return true;
            }
        }

        public bool TryUpdate<T, TId>(TId id, T updatedItem) where T : class, IHasIdProperty<TId>
        {
            using (var crud = new BasicCRUD<T,TId>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryUpdate(id, updatedItem))
                {
                    return false;
                }
                return true;
            }
        }

        private IEnumerable<Lector> GetLectors(IQueryable<Lector> lectors, 
            int id, string fio, string email)
        {
            if (id != 0)
            {
                lectors = lectors.Where(lect => lect.Id == id);
                return lectors;
            }
            if (fio is not null) lectors = lectors.Where(lect => lect.Fio == fio);
            if (email is not null) lectors = lectors.Where(lect => lect.Email == email);
            return lectors.AsEnumerable();
        }

        public bool TryGetLectors(out Lector[] outLectors, int id = 0, string fio = null, string email = null)
        {
            using (var crud = new BasicCRUD<Lector, int>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryGet(out IQueryable<Lector> queryLectors))
                {
                    outLectors = null;
                    return false;
                }
                var lectors = GetLectors(queryLectors, id, fio, email);
                if (!Validation.IsAllLectorsValid(lectors, 
                    out IEnumerable<Lector> validLectors, out string invalidLectorsId))
                {
                    _logger.LogWarning("Invalid lectors in the database with id: " + invalidLectorsId);
                }
                outLectors = validLectors.ToArray();
                return true;
            }
        }

        private IEnumerable<Student> GetStudents(IQueryable<Student> students, int id, string fio,
            string email, string phoneNumber)
        {
            if (id != 0)
            {
                students = students.Where(stud => stud.Id == id);
                return students;
            }
            if (fio is not null) students = students.Where(stud => stud.Fio == fio);
            if (email is not null) students = students.Where(stud => stud.Email == email);
            if (phoneNumber is not null) students = students.Where(stud => stud.PhoneNumber == phoneNumber);
            return students;
        }

        public bool TryGetStudents(out Student[] outStudents, int id = 0, string fio = null, 
            string email = null, string phoneNumber = null)
        {
            using (var crud = new BasicCRUD<Student, int>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryGet(out IQueryable<Student> queryStudents))
                {
                    outStudents = null;
                    return false;
                }
                var students = GetStudents(queryStudents, id, fio, email, phoneNumber);
                if (!Validation.IsAllStudentsValid(students, out IEnumerable<Student> validStudents,
                    out string invalidStudentsId))
                {
                    _logger.LogWarning("Invalid students in the database with id: " + invalidStudentsId);
                }
                outStudents = validStudents.ToArray();
                return true;
            }
        }

        private IEnumerable<Lection> GetLections(IQueryable<Lection> lections, 
            int id, string topic, string date, int lectorId)
        {
            if (id != 0)
            {
                lections = lections.Where(lect => lect.Id == id);
                return lections;
            }
            if (topic is not null) lections = lections.Where(lect => lect.Topic == topic);
            if (date is not null) lections = lections.Where(lect => lect.Date == date);
            if (lectorId != 0) lections = lections.Where(lect => lect.LectorId == lectorId);
            return lections;
        }

        public bool TryGetLections(out Lection[] outLections, int id = 0, string topic = null, 
            string date = null, int lectorId = 0)
        {
            using (var crud = new BasicCRUD<Lection, int>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryGet(out IQueryable<Lection> queryLections))
                {
                    outLections = null;
                    return false;
                }
                var lections = GetLections(queryLections, id, topic, date, lectorId);
                if (!Validation.IsAllLectionsValid(lections, out IEnumerable<Lection> validLections,
                    out string invalidLectionsId))
                {
                    _logger.LogWarning("Invalid lections in the database with id: " + invalidLectionsId);
                }
                outLections = validLections.ToArray();
                return true;
            }
        }

        private IEnumerable<Homework> GetHomeworks(IQueryable<Homework> homeworks, int id, int lectionId,
            int studentId, int mark)
        {
            if (id != 0)
            {
                homeworks = homeworks.Where(work => work.Id == id);                
                return homeworks;
            }
            if (lectionId != 0) homeworks = homeworks.Where(work => work.LectionId == lectionId);
            if (studentId != 0) homeworks = homeworks.Where(work => work.StudentId == studentId);
            if (mark != -1) homeworks = homeworks.Where(work => work.Mark == mark);
            return homeworks;
        }

        public bool TryGetHomeworks(out Homework[] outHomeworks, int id = 0, int lectionId = 0, 
            int studentId = 0, int mark = -1)
        {
            using (var crud = new BasicCRUD<Homework, int>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryGet(out IQueryable<Homework> queryHomeworks))
                {
                    outHomeworks = null;
                    return false;
                }
                var homeworks = GetHomeworks(queryHomeworks, id, lectionId, studentId, mark);
                if (!Validation.IsAllHomeworksValid(homeworks, out IEnumerable<Homework> validHomeworks,
                    out string invalidHomeworksId))
                {
                    _logger.LogWarning("Invalid homeworks in the database with id: " + invalidHomeworksId);
                }
                outHomeworks = validHomeworks.ToArray();
                return true;
            }
        }

        private IEnumerable<Attendance> GetAttendance(IQueryable<Attendance> attendance, int id, int lectionId,
            int studentId)
        {
            if (id != 0)
            {
                attendance = attendance.Where(att => att.Id == id);
                return attendance;
            }
            if (lectionId != 0) attendance = attendance.Where(att => att.LectionId == lectionId);
            if (studentId != 0) attendance = attendance.Where(att => att.StudentId == studentId);
            return attendance;
        }

        public bool TryGetAttendance(out Attendance[] outAttendance, int id = 0, int lectionId = 0, 
            int studentId = 0)
        {
            using (var crud = new BasicCRUD<Attendance, int>(new EducationServerContext(_dbOptions), _logger))
            {
                if (!crud.TryGet(out IQueryable<Attendance> queryAttendance))
                {
                    outAttendance = null;
                    return false;
                }
                var attendance = GetAttendance(queryAttendance, id, lectionId, studentId);
                if (!Validation.IsAllAttendanceValid(attendance,
                    out IEnumerable<Attendance> validAttendance, out string invalidAttendanceId))
                {
                    _logger.LogWarning("Invalid lectors in the database with id: " + invalidAttendanceId);
                }
                outAttendance = validAttendance.ToArray();                
                return true;
            }
        }
    }
}
