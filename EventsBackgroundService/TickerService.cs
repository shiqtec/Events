namespace EventsBackgroundService
{
    public class TickerService
    {
        public event EventHandler<TickerEventArgs>? Ticked;

        public TickerService()
        {
            Ticked += OnEveryOneSecond;
            Ticked += OnEveryFiveSecond;
        }

        public void OnEveryOneSecond(object? sender, TickerEventArgs args)
        {
            Console.WriteLine(args.Time.ToLongTimeString());
        }

        public void OnEveryFiveSecond(object? sender, TickerEventArgs args)
        {
            if(args.Time.Second % 5 == 0)
            {
                Console.WriteLine(args.Time.ToLongTimeString());
            }
        }

        public void OnTick(TimeOnly time)
        {
            Ticked?.Invoke(this, new TickerEventArgs(time));
        }
    }
}
