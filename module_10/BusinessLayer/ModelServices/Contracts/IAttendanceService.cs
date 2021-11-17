using Models.Domain;

namespace BusinessLayer.ModelServices.Contracts
{
    public interface IAttendanceService
    {
        public bool TryAdd(Attendance newAttendance);
        public bool TryUpdate(int id, Attendance updatedAttendance);
        public bool TryDelete(int id);
        public bool TryGet(out Attendance[] outAttendances, int id = 0, int lectionId = 0,
            int studentId = 0);
    }
}
