using MediatR;

namespace EventsMediatR
{
    public class EverySecondHandler : INotificationHandler<TimedNotification>
    {
        private readonly TransientGUIDService _guidService;

        public EverySecondHandler(TransientGUIDService guidService)
        {
            _guidService = guidService;
        }

        public Task Handle(TimedNotification notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(_guidService.guid);
            return Task.CompletedTask;
        }
    }
}
