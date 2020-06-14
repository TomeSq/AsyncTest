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

        static async Task Main(string[] args)
        {
            Console.WriteLine("start AsyncMethod");

            //これは非同期
            //_ = AsyncMethod().ConfigureAwait(false);

            //これは以降が同期
            await AsyncMethod().ConfigureAwait(false);
            Console.WriteLine("end AsyncMethod");

            Console.ReadKey();
        }
    }
}
