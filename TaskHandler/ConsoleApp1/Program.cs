// See https://aka.ms/new-console-template for more information
using TasksHandler;
using TasksGenerator;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;
using System.Collections.Concurrent;
using TaskHandler.Domain;

internal class Program
{
    private static async Task Main(string[] args)
    {



        var qTasks = new ConcurrentQueue<Func<Task>>();
        foreach (var num in Enumerable.Range(1, 7))
        {
            qTasks.Enqueue(() => Tasks.TaskWithTimeout(3000, num));
        }


        var sw = new Stopwatch();
        var taskHandler = new AsyncTaskController(3,EnumCollectionType.ConcurrentQueue);
        var tasks = new List<Task>();
        foreach (var num in Enumerable.Range(1, 7))
        {
            tasks.Add(taskHandler.ProcessNextTaskAsync(async () =>
            {
                await Tasks.TaskWithTimeout(3000, num);
                return Task.Factory.StartNew(() => num);
            }));
        }
       
            await Task.WhenAll(tasks);

        Console.ReadKey();
    }
}