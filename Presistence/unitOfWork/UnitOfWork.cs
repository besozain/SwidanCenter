using Domain.Contracts;
using Domain.Entities;
using Presistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presistence.Repositories;

namespace Presistence.unitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;


        readonly Dictionary<string, object> _repositories = new Dictionary<string, object>();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }


        public IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>
        {
            if (_repositories.ContainsKey(typeof(T).Name))
            {
                return (IGenericRepository<T, TKey>)_repositories[typeof(T).Name];
            }
            var repository = new GenericRepository<T, TKey>(_context);
            _repositories.Add(typeof(T).Name, repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
