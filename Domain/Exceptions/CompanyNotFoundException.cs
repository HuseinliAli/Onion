namespace Entities.Exceptions;

public sealed class CompanyNotFoundException : NotFoundException
{
    public CompanyNotFoundException(Guid companyId)
        :base($"The company with id: {companyId} doesn't exists in the database.")
    {
        
    }
}

public sealed class EmployeeNotFoundException : NotFoundException
{
    public EmployeeNotFoundException(Guid employeeId)
        : base($"The employee with id: {employeeId} doesn't exists in the database.")
    {

    }
}