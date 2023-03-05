using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using TaskHandler.Domain;

public class AsyncTaskController
{

    private readonly IConcurrentCollection<Func<Task>> _taskQueue;
    private readonly SemaphoreSlim _semaphore;

    public AsyncTaskController(int maxConcurrentTasks,EnumCollectionType collectionType)
    {
        _semaphore = new SemaphoreSlim(maxConcurrentTasks, maxConcurrentTasks);
        _taskQueue = CreateConcurrentCollection(collectionType);
    }

    private IConcurrentCollection<Func<Task>> CreateConcurrentCollection(EnumCollectionType collectionType)
    {
         return collectionType switch
        {
            EnumCollectionType.ConcurrentQueue => new MyConcurrentQueue<Func<Task>>(),
            EnumCollectionType.ConcurrentStack => new MyConcurrentStack<Func<Task>>()
        };
    }

    public async Task EnqueueAsync(Func<Task> taskFunc)
    {
        _taskQueue.NextItemIntoCollection(taskFunc);
        await TryStartNextAsync();
    }


    public async Task<TResult> EnqueueAsync<TResult>(Func<Task<TResult>> taskFunc)
    {
        var tcs = new TaskCompletionSource<TResult>();
        _taskQueue.NextItemIntoCollection(async () =>
        {
            try
            {
                var result = await taskFunc();
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
                throw; // to be liskov compliant, all tasks must succeed
            }
        });

        await TryStartNextAsync();

        return await tcs.Task;
    }

    private async Task TryStartNextAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            while (_taskQueue.TryRemoveItemFromCollection(out var taskFunc))
            {
                await taskFunc();
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}