using Models;
using Models.Database;

namespace DataLayer
{
    public interface IDataAccess
    {
        public bool TryAdd<T, TId>(T itemToAdd) where T : class, IHasIdProperty<TId>;
        public bool TryDelete<T, TId>(TId id) where T : class, IHasIdProperty<TId>;
        public bool TryUpdate<T, TId>(TId id, T updatedItem) where T : class, IHasIdProperty<TId>;
        public bool TryGetLectors(out Lector[] outLectors, int id = 0, string fio = null, string email = null);
        public bool TryGetStudents(out Student[] outStudents, int id = 0, string fio = null,
            string email = null, string phoneNumber = null);
        public bool TryGetLections(out Lection[] outLections, int id = 0, string topic = null,
            string date = null, int lectorId = 0);
        public bool TryGetHomeworks(out Homework[] outHomeworks, int id = 0, int lectionId = 0,
            int studentId = 0, int mark = -1);
        public bool TryGetAttendance(out Attendance[] outAttendance, int id = 0, int lectionId = 0,
            int studentId = 0);
    }
}
