using System.Collections.Concurrent;

namespace TaskHandler.Domain
{
    public interface IConcurrentCollection<T> : IProducerConsumerCollection<T>, IReadOnlyCollection<T>
    {
        void NextItemIntoCollection(T item);

        bool TryRemoveItemFromCollection(out T fetchedItem);

    }
}