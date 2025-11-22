using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<T, TKey> GetRepository<T, TKey>() where T : BaseEntity<TKey>;
        public Task<int> SaveChangesAsync();
    }
}
