using AutoMapper;
using DataLayer;
using Microsoft.Extensions.Logging;

namespace BusinessLayer.ModelServices.Implementations
{
    public abstract class BaseService<TSource, TDestination, TId>
         where TDestination : class, Models.IHasIdProperty<TId>

    {
        protected IMapper _mapper;
        protected IDataAccess _dataAccess;
        public BaseService(IMapper mapper, IDataAccess dataAccess)
        {
            _mapper = mapper;
            _dataAccess = dataAccess;
        }

        protected abstract bool IsValid(TSource item);
        protected abstract bool IsValid(TId id);

        public bool TryAdd(TSource newItem)
        {
            if (!IsValid(newItem))
            {
                return false;
            }
            CRUD crud = new CRUD(_mapper, _dataAccess);
            return crud.TryAdd<TSource, TDestination, TId>(newItem);
        }

        public bool TryUpdate(TId id, TSource updatedItem)
        {
            if (!IsValid(id) ||
                !IsValid(updatedItem))
            {
                return false;
            }
            CRUD crud = new CRUD(_mapper, _dataAccess);
            return crud.TryUpdate<TSource, TDestination, TId>(id, updatedItem);
        }

        public bool TryDelete(TId id)
        {
            if (!IsValid(id))
            {
                return false;
            }
            CRUD crud = new CRUD(_mapper, _dataAccess);
            return crud.TryDelete<TDestination, TId>(id);
        }
    }
}
