﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        private ObjectCache cache = MemoryCache.Default;
        private List<T> items;
        private string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;
            items = cache[className] as List<T>;
            if (items == null)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T item)
        {
            items.Add(item);
        }

        public void Update(T item)
        {
            T itemToUpdate = items.Find(i => i.Id == item.Id);

            if (itemToUpdate != null)
            {
                itemToUpdate = item;
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }

        public T Find(string id)
        {
            T item = items.Find(i => i.Id == id);

            if (item != null)
            {
                return item;
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }

        public void Delete(string id)
        {
            T itemToDelete = items.Find(i => i.Id == id);

            if (itemToDelete != null)
            {
                items.Remove(itemToDelete);
            }
            else
            {
                throw new Exception(className + " not found");
            }
        }
    }
}
