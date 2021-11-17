using Models.Domain;

namespace BusinessLayer.ModelServices.Contracts
{
    public interface IHomeworksService
    {
        public bool TryAdd(Homework newHomework);
        public bool TryUpdate(int id, Homework updatedHomework);
        public bool TryDelete(int id);
        public bool TryGet(out Homework[] outHomeworks, int id = 0, int lectionId = 0,
           int studentId = 0, int mark = -1);
    }
}
