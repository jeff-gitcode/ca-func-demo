namespace Application.Abstraction
{
    public interface ITimer : IDisposable, IAsyncDisposable
    {
        bool Change(TimeSpan dueTime, TimeSpan period);
    }
}
