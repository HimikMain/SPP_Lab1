using System;
using System.Threading;

namespace SPPLab1
{
    class Program
    {
		static int num = 1;
		static object locker = new object();
		static void count()
		{
			lock (locker)
			{
				int j = 1;
				for (int i = 0; i < 3; i++)
				{
					Console.WriteLine("Поток №{0}: {1}", num, j);
					j++;
					Thread.Sleep(50);
				}
				num++;
			}
		}
		static void Main(string[] args)
        {
            TaskQueue taskQueue = new TaskQueue(3);
            for (int i = 0; i < 3; i++)
            {
                taskQueue.EnqueueTask(count);
			}
            Console.ReadLine();
        }
    }
}
