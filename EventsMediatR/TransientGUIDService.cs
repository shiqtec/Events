namespace EventsMediatR
{
    public class TransientGUIDService
    {
        public Guid guid { get; } = Guid.NewGuid();
    }
}
