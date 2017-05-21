using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Permissions;
namespace Ngram
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ngram.Instance.runFirst();
            ThreadStart myThreadDelegate = new ThreadStart(DoWork);
            Thread myThread = new Thread(myThreadDelegate);
            myThread.Start();
            Thread.Sleep(100);
            Console.WriteLine("Main - aborting my thread.");
            myThread.Abort();
            myThread.Join();
            Console.WriteLine("Main ending.");
        }
        public static void DoWork()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.WriteLine("Thread - working.");
                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException e)
            {
                Console.WriteLine("Thread - caught ThreadAbortException - resetting.");
                Console.WriteLine("Exception message: {0}", e.Message);
                Thread.ResetAbort();
            }
            Console.WriteLine("Thread - still alive and working.");
            Thread.Sleep(1000);
            Console.WriteLine("Thread - finished working.");
        }
    }
}
