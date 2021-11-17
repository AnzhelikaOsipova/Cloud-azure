using Models.Domain;

namespace BusinessLayer.ModelServices.Contracts
{
    public interface IStudentsService
    {
        public bool TryAdd(Student newStudent);
        public bool TryUpdate(int id, Student updatedStudent);
        public bool TryDelete(int id);
        public bool TryGet(out Student[] outStudents, int id = 0, string fio = null, string email = null, string phoneNumber = null);
    }
}
