using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskHandler.Domain
{
    public class MyConcurrentQueue<T> : ConcurrentQueue<T>, IConcurrentCollection<T>
    {
        public void NextItemIntoCollection(T item)
        {
            Enqueue(item);
        }

        public bool TryRemoveItemFromCollection(out T fetchedItem)
        {
            if (TryDequeue(out T fetchedItemConcQueue))
            {
                fetchedItem = fetchedItemConcQueue;
                return true;
            }
            fetchedItem = default;
            return false;
        }
    }
}
