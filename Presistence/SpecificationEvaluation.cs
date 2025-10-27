using Domain.Contracts.SpecificationContracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presistence
{
    static public class SpecificationEvaluation
    {
        static public IQueryable<T> ApplySpecification<T>(this IQueryable<T> query, ISpecification<T> specification) where T : BaseEntity
        {

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            foreach (var include in specification.Includes)
            {
                query = query.Include(include);
            }
            foreach (var includeStr in specification.IncludeStrings)
            {
                query = query.Include(includeStr);
            }
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take ?? 10); // Default take to 10 if not specified
            }

            if (specification.IsTracking.HasValue)
            {
                query = specification.IsTracking.Value ? query.AsTracking() : query.AsNoTracking();
            }

            return query;
        }
    }
}
