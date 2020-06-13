using System;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static async Task AsyncMethod()
        {
            await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
            Console.WriteLine("Done!");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("start AsyncMethod");
            var task = AsyncMethod();
            task.Wait();
            Console.WriteLine("end AsyncMethod");
        }
    }
}
