﻿using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            //SE CREA UNA CONSULTA IQUERYABLE APARTIR DEL DBSET DEL CONTEXTO
            IQueryable<T> query = dbSet;

            //se aplica el filtro si es que existe
            if (filter != null)
            {
                query = query.Where(filter);
            }
            //se incluyen las propiedades relacionadas
            if (includeProperties != null)
            {
                //se incluyen las propiedades relacionadas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            //se ordena la consulta si es que existe
            if (orderBy != null)
            {
                //se ordena la consulta y se convierte a lista
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            //SE CREA UNA CONSULTA IQUERYABLE APARTIR DEL DBSET DEL CONTEXTO
            IQueryable<T> query = dbSet;

            //se aplica el filtro si es que existe
            if (filter != null)
            {
                query = query.Where(filter);
            }

            //se incluyen las propiedades relacionadas
            if (includeProperties != null)
            {
                //se incluyen las propiedades relacionadas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();


        }

        public void Remove(int id)
        {
            T entityToRemove = dbSet.Find(id);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

    }
}
