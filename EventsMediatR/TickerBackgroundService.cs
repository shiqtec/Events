using EventsMediatR;
using MediatR;

namespace EventsBackgroundService
{
    public class TickerBackgroundService : BackgroundService
    {
        private readonly IMediator _mediator;

        public TickerBackgroundService(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var timeNow = TimeOnly.FromDateTime(DateTime.Now);
                await _mediator.Publish(new TimedNotification(timeNow), stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
