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
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification);
        Task<T> GetByIdAsync(ISpecification<T> specification);
        Task<int> CountAsync(ISpecification<T> specification);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        public IQueryable<T> GetAllQueryable(ISpecification<T> spec);
    }
}
