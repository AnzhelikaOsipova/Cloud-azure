using AutoMapper;
using DataLayer;
using Microsoft.Extensions.Logging;
using Models.Domain;
using BusinessLayer.ModelServices.Contracts;

namespace BusinessLayer.ModelServices.Implementations
{
    public class StudentsService: BaseService<Student, Models.Database.Student, int>, IStudentsService
    {
        private ILogger _logger;
        public StudentsService(IMapper mapper, IDataAccess dataAccess, ILogger logger):
            base(mapper, dataAccess)
        {
            _logger = logger;
        }

        protected override bool IsValid(Student item)
        {
            if (item.Id < 0)
            {
                _logger.LogWarning("Invalid student parameters: id mustn't be < 0, but was " + item.Id);
                return false;
            }
            if (item.Fio is null ||
                item.Fio == "")
            {
                _logger.LogWarning("Invalid student parameters: fio mustn't be empty");
                return false;
            }
            if (item.Email is null)
            {
                _logger.LogWarning("Invalid student parameters: invalid email");
                return false;
            }
            if (item.PhoneNumber is null)
            {
                _logger.LogWarning("Invalid student parameters: invalid phone number");
                return false;
            }
            return true;
        }

        protected override bool IsValid(int id)
        {
            if (id < 1)
            {
                _logger.LogWarning("Invalid student parameters: id mustn't be < 1, but was " + id);
                return false;
            }
            return true;
        }
        
        public bool TryGet(out Student[] outStudents, int id = 0, string fio = null, string email = null, string phoneNumber = null)
        {
            CRUD crud = new CRUD(_mapper, _dataAccess);
            return crud.TryGetStudents(out outStudents, id, fio, email, phoneNumber);
        }
    }
}
