using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
