﻿using System.Linq.Expressions;

namespace APICatalogo.Repository
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        T GetByid(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);  
    }
}
