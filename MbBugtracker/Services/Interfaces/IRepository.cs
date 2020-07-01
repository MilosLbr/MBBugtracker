using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MbBugtracker.Services.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // read
        Task<TEntity> GetById(int Id);
        Task<IEnumerable<TEntity>> GetAll();
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        // add
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);


        // remove
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
