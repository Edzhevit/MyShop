using System;
using System.Data.Entity;
using System.Linq;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.DataAccess.SQL.Data;

namespace MyShop.DataAccess.SQL.Repository
{
    public class SqlRepository<T> : IRepository<T> where T: BaseEntity
    {

        internal DataContext context;
        internal DbSet<T> dbSet;

        public SqlRepository(DataContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Insert(T item)
        {
            dbSet.Add(item);
        }

        public void Update(T item)
        {
            dbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
        }

        public T Find(string id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> Collection()
        {
            return dbSet;
        }

        public void Delete(string id)
        {
            var item = Find(id);
            if (context.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }

            dbSet.Remove(item);
        }
    }
}
