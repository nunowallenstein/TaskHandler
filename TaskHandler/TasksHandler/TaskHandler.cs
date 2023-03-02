using System.Collections.Concurrent;

namespace TasksHandler
{
    //Exception handling

    public class TaskHandler
    {
        private readonly int _limitOfTasks;
        private readonly Queue<Func<Task>> _tasksToExecute; //assuming for now that there is not Type of T

        public ConcurrentBag<Task> Results { get; private set; }= new ConcurrentBag<Task>();

        public TaskHandler(int limitOfTasks, Queue<Func<Task>> tasksToExecute)
        {
            _limitOfTasks = limitOfTasks;
            _tasksToExecute = tasksToExecute;
        }

        
        public async Task Execute()
        {
            var locker = new object();
            var batchExecutingTasks = new ConcurrentBag<Task>();
            while (_tasksToExecute.Count > 0)
            { 
                if (batchExecutingTasks.Count < _limitOfTasks)
                {
                    batchExecutingTasks.Add(_tasksToExecute.Dequeue()());
                }
                else
                {                
                    await Task.WhenAny(batchExecutingTasks).ContinueWith(t =>
                    {
                        batchExecutingTasks.TryTake(out t);
                        Results.Add(t);
                    });
                }

            }
            if (batchExecutingTasks.Count > 0)
            {
                await Task.WhenAll(batchExecutingTasks);

                foreach (var task in batchExecutingTasks)
                {
                    Results.Add(task);
                }
                batchExecutingTasks.Clear();

            }
        }

    }
}