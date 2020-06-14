using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var task = HeavyMethod1();

            HeavyMethod2();

            Console.WriteLine(task.Result);

            Console.ReadKey();
        }

        static async Task<string> HeavyMethod1()
        {
            Console.WriteLine("すごく重い処理その1(´・ω・｀)はじまり");
            await Task.Delay(5000).ConfigureAwait(false);
            Console.WriteLine("すごく重い処理その1(´・ω・｀)おわり");
            return "hoge";
        }

        static void HeavyMethod2()
        {
            Console.WriteLine("すごく重い処理その2(´・ω・｀)はじまり");
            Thread.Sleep(3000);
            Console.WriteLine("すごく重い処理その2(´・ω・｀)おわり");
        }
    }
}
