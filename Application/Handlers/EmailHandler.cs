using Application.Notifications;
using Contracts.Logging;
using MediatR;

namespace Application.Handlers;

internal sealed class EmailHandler(ILoggerManager logger) : INotificationHandler<CompanyDeleteNotification>
{
    public async Task Handle(CompanyDeleteNotification notification, CancellationToken cancellationToken)
    {
        logger.LogWarning($"Delete action for company with id: {notification.Id}");
        await Task.CompletedTask;
    }
}