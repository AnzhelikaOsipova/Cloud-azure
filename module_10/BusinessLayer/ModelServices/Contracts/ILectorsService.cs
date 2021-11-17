using Models.Domain;

namespace BusinessLayer.ModelServices.Contracts
{
    public interface ILectorsService
    {
        public bool TryAdd(Lector newLector);
        public bool TryUpdate(int id, Lector updatedLector);
        public bool TryDelete(int id);
        public bool TryGet(out Lector[] outLectors, int id = 0, string fio = null, string email = null);

    }
}
