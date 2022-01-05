using Microsoft.EntityFrameworkCore;
using SampleRestAPI.Database;
using SampleRestAPI.Database.Entity;

namespace SampleRestAPI.Repositories
{
    public abstract class AbstractRepository<T> where T : class, IIdEntity
    {
        protected AppDbContext dbContext;
        private DbSet<T> defaultTable;

        public AbstractRepository(AppDbContext dbContext, DbSet<T> defaultTable)
        {
            this.dbContext = dbContext;
            this.defaultTable = defaultTable;
        }

        internal IEnumerable<T> GetAll()
        {
            return defaultTable.ToArray();
        }

        internal void Update(int id, T item)
        {
            var currentItem = defaultTable.FirstOrDefault(x => x.Id == id);
            if (currentItem == null) throw new Services.Exceptions.NotFoundException();
            defaultTable.Remove(currentItem);
            defaultTable.Add(item);
            dbContext.SaveChanges();
        }
        internal void Update(int id, Dictionary<string, object> changes)
        {
            var currentItem = defaultTable.FirstOrDefault(x => x.Id == id);
            if (currentItem == null) throw new Services.Exceptions.NotFoundException();
            foreach (var change in changes)
            {
                currentItem.GetType().GetProperty(change.Key).SetValue(currentItem, change.Value, null);
            }
            dbContext.SaveChanges();
        }

        internal T Insert(T item)
        {
            defaultTable.Add(item);
            dbContext.SaveChanges();
            return item;
        }

        internal void Delete(int id)
        {
            var item = defaultTable.FirstOrDefault(x => x.Id == id);
            if (item == null) throw new Services.Exceptions.NotFoundException();
            defaultTable.Remove(item);
            dbContext.SaveChanges();
        }

        internal T? GetOne(int id)
        {
            return defaultTable.FirstOrDefault(x => x.Id == id);
        }
    }
}
