using AutoMapper;
using Models.Domain;
using DataLayer;
using Microsoft.Extensions.Logging;
using BusinessLayer.ModelServices.Contracts;

namespace BusinessLayer.ModelServices.Implementations
{
    public class LectorsService: BaseService<Lector, Models.Database.Lector, int>, ILectorsService
    {
        private ILogger _logger;
        public LectorsService(IMapper mapper, IDataAccess dataAccess, ILogger logger):
            base(mapper, dataAccess)
        {
            _logger = logger;
        }

        protected override bool IsValid(Lector item)
        {
            if (item.Id < 0)
            {
                _logger.LogWarning("Invalid lector parameters: id mustn't be < 0, but was " + item.Id);
                return false;
            }
            if (item.Fio is null ||
                item.Fio == "")
            {
                _logger.LogWarning("Invalid lector parameters: fio mustn't be empty");
                return false;
            }
            if (item.Email is null)
            {
                _logger.LogWarning("Invalid lector parameters: invalid email");
                return false;
            }
            return true;
        }

        protected override bool IsValid(int id)
        {
            if (id < 1)
            {
                _logger.LogWarning("Invalid lector parameters: id mustn't be < 1, but was " + id);
                return false;
            }
            return true;
        }
        
        public bool TryGet(out Lector[] outLectors, int id = 0, string fio = null, string email = null)
        {
            CRUD crud = new CRUD(_mapper, _dataAccess);
            return crud.TryGetLectors(out outLectors, id, fio, email);
        }
    }
}
