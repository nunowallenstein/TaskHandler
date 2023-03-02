// See https://aka.ms/new-console-template for more information
using TasksHandler;
using TasksGenerator;
internal class Program
{
    private static async Task Main(string[] args)
    {



        var qTasks = new Queue<Func<Task>>();

        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 1));
        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 2));
        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 3));
        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 5));
        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 6));
        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 7));
        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 8));
        qTasks.Enqueue(() => Tasks.TaskWithTimeout(10000, 9));


        var taskHandler = new TaskHandler(2, qTasks);
        await taskHandler.Execute();

        Console.ReadKey();
    }
}