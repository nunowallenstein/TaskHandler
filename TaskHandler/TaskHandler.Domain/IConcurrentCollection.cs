using System.Collections.Concurrent;

namespace TaskHandler.Domain
{
    public interface IConcurrentCollection<T> : IProducerConsumerCollection<T>, IReadOnlyCollection<T>
    {
        
    }
}