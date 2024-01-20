using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Repositories.Extensions.Utility;

namespace Repositories.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> query, uint minAge, uint maxAge)
            =>query.Where(e=>e.Age>=minAge && e.Age<=maxAge);
        public static IQueryable<Employee> Search(this IQueryable<Employee> query, string searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm))
                return query;
            return query.Where(e => e.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(x => x.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(x => x.Name);

            return employees.OrderBy(orderQuery);
        }
    }
}
