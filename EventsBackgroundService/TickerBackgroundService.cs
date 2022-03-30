namespace EventsBackgroundService
{
    public class TickerBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Message");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
