using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public class AsyncTaskController
{
    private readonly int _maxConcurrentTasks;
    private readonly ConcurrentQueue<Func<Task>> _taskQueue = new ConcurrentQueue<Func<Task>>();
    private readonly SemaphoreSlim _semaphore;

    public AsyncTaskController(int maxConcurrentTasks)
    {
        _maxConcurrentTasks = maxConcurrentTasks;
        _semaphore = new SemaphoreSlim(maxConcurrentTasks, maxConcurrentTasks);
    }

    public async Task EnqueueAsync(Func<Task> taskFunc)
    {
        _taskQueue.Enqueue(taskFunc);
        await TryStartNextAsync();
    }


    public async Task<TResult> EnqueueAsync<TResult>(Func<Task<TResult>> taskFunc)
    {
        var tcs = new TaskCompletionSource<TResult>();
        _taskQueue.Enqueue(async () =>
        {
            try
            {
                var result = await taskFunc();
                tcs.SetResult(result);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
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
            while (_taskQueue.TryDequeue(out var taskFunc))
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