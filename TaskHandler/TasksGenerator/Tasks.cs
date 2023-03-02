using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksGenerator
{
    static public class Tasks
    {
      public async static Task TaskWithTimeout(int timetout,int id)
        {
            await Task.Delay(timetout);
            Console.WriteLine($"Task {id} finished");
        }
    }
}
