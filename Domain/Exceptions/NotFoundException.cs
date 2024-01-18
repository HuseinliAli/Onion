using Entities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions;
public abstract class NotFoundException : Exception
{
    protected NotFoundException(string message):base(message)
    {

    }
}
public sealed class CompanyCollectionBadRequest : BadRequestException
{
    public CompanyCollectionBadRequest():base("Company collection sent from a client is null")
    {
        
    }
}

public sealed class MaxAgeRangeBadRequestException : BadRequestException
{
    public MaxAgeRangeBadRequestException() : base("Max age cant be less than min age")
    {
    }
}