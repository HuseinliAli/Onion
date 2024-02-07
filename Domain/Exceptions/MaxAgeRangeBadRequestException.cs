namespace Entities.Exceptions;

public sealed class MaxAgeRangeBadRequestException : BadRequestException
{
    public MaxAgeRangeBadRequestException() : base("Max age cant be less than min age")
    {
    }
}
