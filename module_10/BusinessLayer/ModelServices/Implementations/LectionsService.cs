using AutoMapper;
using DataLayer;
using Microsoft.Extensions.Logging;
using Models.Domain;
using BusinessLayer.ModelServices.Contracts;

namespace BusinessLayer.ModelServices.Implementations
{
    public class LectionsService: BaseService<Lection, Models.Database.Lection, int>, ILectionsService
    {
        private ILogger _logger;
        public LectionsService(IMapper mapper, IDataAccess dataAccess, ILogger logger):
            base(mapper, dataAccess)
        {
            _logger = logger;
        }

        protected override bool IsValid(Lection item)
        {
            if (item.Id < 0)
            {
                _logger.LogWarning("Invalid lection parameters: id mustn't be < 0, but was " + item.Id);
                return false;
            }
            if (item.Topic is null ||
                item.Topic == "")
            {
                _logger.LogWarning("Invalid Lection parameters: topic mustn't be empty");
                return false;
            }
            if (item.LectorId < 1)
            {
                _logger.LogWarning("Invalid lection parameters: lectorId mustn't be < 1, but was " + item.LectorId);
                return false;
            }
            if (item.Date is null)
            {
                _logger.LogWarning("Invalid lection parameters: invalid date");
                return false;
            }
            return true;
        }

        protected override bool IsValid(int id)
        {
            if (id < 1)
            {
                _logger.LogWarning("Invalid lection parameters: id mustn't be < 1, but was " + id);
                return false;
            }
            return true;
        }
        
        public bool TryGet(out Lection[] outLections, int id = 0, string topic = null,
            string date = null, int lectorId = 0)
        {
            CRUD crud = new CRUD(_mapper, _dataAccess);
            return crud.TryGetLections(out outLections, id, topic, date, lectorId);
        }
    }
}
