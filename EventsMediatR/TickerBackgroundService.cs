namespace EventsBackgroundService
{
    public class TickerBackgroundService : BackgroundService
    {
        private readonly TickerService _tickerService;

        public TickerBackgroundService(TickerService tickerService)
        {
            _tickerService = tickerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _tickerService.OnTick(TimeOnly.FromDateTime(DateTime.Now));
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
