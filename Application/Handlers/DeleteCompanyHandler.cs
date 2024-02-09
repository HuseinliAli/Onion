using Application.Notifications;
using Contracts.Managers;
using Entities.Exceptions;
using MediatR;

namespace Application.Handlers;

internal sealed class DeleteCompanyHandler(IRepositoryManager repository) :INotificationHandler<CompanyDeleteNotification>
{
    public async Task Handle(CompanyDeleteNotification notification, CancellationToken cancellationToken)
    {
        var companyEntity = await repository.Company.GetCompanyAsync(notification.Id, notification.ChangeTracker);
        if (companyEntity is null)
            throw new CompanyNotFoundException(notification.Id);

        repository.Company.DeleteCompany(companyEntity);
        await repository.SaveAsync(); 
    }
}
