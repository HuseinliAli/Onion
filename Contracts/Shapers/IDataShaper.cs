using Domain.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Shapers
{
    public interface IDataShaper<T>
    {
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entites, string fieldsString);

        ShapedEntity ShapeData(T entity, string fieldsString); 
    }
}
