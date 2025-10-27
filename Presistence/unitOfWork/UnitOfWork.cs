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


        public IGenericRepository<T> GetRepository<T>() where T : BaseEntity
        {
            if (_repositories.ContainsKey(typeof(T).Name))
            {
                return (IGenericRepository<T>)_repositories[typeof(T).Name];
            }
            var repository = new GenericRepository<T>(_context);
            _repositories.Add(typeof(T).Name, repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
