using Domain.Contracts.SpecificationContracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<T, TKey> where T : BaseEntity<TKey>
    {
        #region Normal Methods
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        #endregion

        #region Specification Methods
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T , TKey> specification);
        Task<T> GetByIdAsync(ISpecification<T, TKey> specification);
        Task<int> CountAsync(ISpecification<T, TKey> specification);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T, TKey> spec);
        public IQueryable<T> GetAllQueryable(ISpecification<T, TKey> spec);
        #endregion
    }
}
