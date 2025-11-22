using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Presistence.Data;

namespace Presistence
{
    public class RawSqlExecutor : IRawSqlExecutor
    {
        readonly AppDbContext _context;
        public RawSqlExecutor(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<T>> QueryAsync<T>(string sql, params object[] parameters) where T : class
            => await _context.Database.SqlQueryRaw<T>(sql, parameters).ToListAsync();

        public async Task<T?> QuerySingleAsync<T>(string sql, params object[] parameters) where T : class
        {
            var result = await _context.Database.SqlQueryRaw<T>(sql, parameters).ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task<int> ExecuteAsync(string sql, params object[] parameters)
            => await _context.Database.ExecuteSqlRawAsync(sql, parameters);
    }
}
