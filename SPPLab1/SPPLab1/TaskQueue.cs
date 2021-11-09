using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SPPLab1
{
    public delegate void TaskDelegate();

    class TaskQueue
    {
        static object locker = new object();
        private BlockingCollection<TaskDelegate> Tasks = new BlockingCollection<TaskDelegate>(new ConcurrentQueue<TaskDelegate>());
        private int queueCount;

        public TaskQueue(int queueCount)
        {
            this.queueCount = queueCount;
            for (int i = 0; i < queueCount; i++)
            {
                var thread = new Thread(threadWork) { IsBackground = true }; ;
                thread.Start();
            }
        }

        public void threadWork()
        {
            lock (locker)
            {
                while (true)
                {
                    var task = Tasks.Take();
                    try
                    {
                        task();
                    }
                    catch (ThreadStateException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (ThreadAbortException ex)
                    {
                        Thread.ResetAbort();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public void EnqueueTask(TaskDelegate task)
        {
            this.Tasks.Add(task);
        }
    }
}
