using System.Data.Entity;
using System.Linq;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.SQL.Data;

namespace MyShop.DataAccess.SQL.Repository
{
    public class SqlRepository<T> : IRepository<T> where T: BaseEntity
    {

        internal DataContext Context;
        internal DbSet<T> DbSet;

        public SqlRepository(DataContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<T>();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Insert(T item)
        {
            DbSet.Add(item);
        }

        public void Update(T item)
        {
            DbSet.Attach(item);
            Context.Entry(item).State = EntityState.Modified;
        }

        public T Find(string id)
        {
            return DbSet.Find(id);
        }

        public IQueryable<T> Collection()
        {
            return DbSet;
        }

        public void Delete(string id)
        {
            var item = Find(id);
            if (Context.Entry(item).State == EntityState.Detached)
            {
                DbSet.Attach(item);
            }

            DbSet.Remove(item);
        }
    }
}
