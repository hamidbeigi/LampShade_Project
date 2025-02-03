using _0_Framework.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _0_Framework.Infrastructure
{
    public class RepositoryBase<TKey, T> : IRepository<TKey, T> where T : class
    {
        private readonly DbContext _shopContext;

        public RepositoryBase(DbContext shopContext)
        {
            _shopContext = shopContext;
        }

        public void Create(T entity)
        {
            _shopContext.Add<T>(entity);
        }

        public bool Exists(Expression<Func<T, bool>> expression)
        {
            return _shopContext.Set<T>().Any(expression);
        }

        public T Get(TKey id)
        {
            return _shopContext.Find<T>(id);
        }

        public List<T> Get()
        {
            return _shopContext.Set<T>().ToList();
        }

        public void SaveChanges()
        {
            _shopContext.SaveChanges();
        }
    }
}
