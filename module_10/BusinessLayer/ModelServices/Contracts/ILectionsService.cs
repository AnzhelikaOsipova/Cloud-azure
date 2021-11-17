using Models.Domain;

namespace BusinessLayer.ModelServices.Contracts
{
    public interface ILectionsService
    {
        public bool TryAdd(Lection newLection);
        public bool TryUpdate(int id, Lection updatedLection);
        public bool TryDelete(int id);
        public bool TryGet(out Lection[] outLections, int id = 0, string topic = null,
            string date = null, int lectorId = 0);
    }
}
