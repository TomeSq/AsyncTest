using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTest
{
    class Program
    {
        static async Task Main(string[] args)
        {

            using (var cts = new CancellationTokenSource())
            {
                try
                {

                    cts.CancelAfter(1000);
                    var task = HeavyMethod1(cts.Token);
                    cts.Cancel();

                    HeavyMethod2();

                    await task;
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine($"タスクがキャンセルされました。{ex.Message}");
                }
            }
            Console.ReadKey();
        }

        static async Task HeavyMethod1(CancellationToken ct)
        {
            Console.WriteLine("すごく重い処理その1(´・ω・｀)はじまり");
            //awaitがついているのでDelayが終わるまで次の処理は実行されない
            await Task.Delay(5000).ConfigureAwait(false);

            //自分でチェックしないとタスクキャンセルの例外を投げられない
//            ct.ThrowIfCancellationRequested();

            Console.WriteLine("すごく重い処理その1(´・ω・｀)おわり");
        }

        static void HeavyMethod2()
        {
            Console.WriteLine("すごく重い処理その2(´・ω・｀)はじまり");
            Thread.Sleep(3000);
            Console.WriteLine("すごく重い処理その2(´・ω・｀)おわり");
        }
    }
}
