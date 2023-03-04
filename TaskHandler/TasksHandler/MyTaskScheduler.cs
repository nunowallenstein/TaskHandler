using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksHandler
{
    public class MyTaskScheduler : TaskScheduler
    {

        public MyTaskScheduler() { }
       
        protected override IEnumerable<Task>? GetScheduledTasks()
        {
            throw new NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            throw new NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }
    }
}
