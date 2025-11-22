using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IRawSqlExecutor
    {
        Task<List<T>> QueryAsync<T>(string sql, params object[] parameters) where T : class;
        Task<T?> QuerySingleAsync<T>(string sql, params object[] parameters) where T : class;
        Task<int> ExecuteAsync(string sql, params object[] parameters);
    }
}
