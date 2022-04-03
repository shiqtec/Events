namespace EventsBackgroundService
{
    public class TickerEventArgs
    {
        public TickerEventArgs(TimeOnly time)
        {
            Time = time;
        }

        public TimeOnly Time { get; }
    }
}
