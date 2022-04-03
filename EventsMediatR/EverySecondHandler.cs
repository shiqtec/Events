using MediatR;

namespace EventsMediatR
{
    public class EverySecondHandler : INotificationHandler<TimedNotification>
    {
        public Task Handle(TimedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification.Time.ToLongTimeString());
            return Task.CompletedTask;
        }
    }
}
