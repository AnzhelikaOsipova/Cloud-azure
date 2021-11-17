using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Linq;

namespace DataLayer
{
    public class BasicCRUD<T, TId>: BaseContext<DbContext> 
        where T: class, IHasIdProperty<TId>
    {
        private ILogger _logger;
        public BasicCRUD(DbContext dbContext, ILogger logger):
            base(dbContext)
        {
            _logger = logger;
        }

        public bool TryAdd(T itemToAdd)
        {
            try
            {
                WorkingContext.Set<T>().Add(itemToAdd);
                WorkingContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public bool TryDelete(TId id)
        {
            try
            {
                var items = WorkingContext.Set<T>();
                T itemToDelete = items.Find(id);
                if (itemToDelete is not null)
                {
                    items.Remove(itemToDelete);
                    WorkingContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public bool TryUpdate(TId id, T updatedItem)
        {
            try
            {
                var items = WorkingContext.Set<T>();
                T itemToUpdate = items.AsNoTracking().Where(item => item.Id.Equals(id)).FirstOrDefault();
                if (itemToUpdate is not null)
                {
                    updatedItem.Id = itemToUpdate.Id;
                    items.Update(updatedItem);
                    WorkingContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return false;
            }
        }

        public bool TryGet(out IQueryable<T> outItems)
        {
            try
            {
                outItems = WorkingContext.Set<T>().AsNoTracking().AsQueryable();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                outItems = null;
                return false;
            }
        }

    }
}
