using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AsyncCancel;

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
//                    var task = HeavyMethod1(cts.Token);

                    HeavyMethod2();
                    cts.CancelAfter(TimeSpan.FromSeconds(5));
                    //CancelAfterは直前に設定しないと実行した時点からのカウントになる
//                    await Task.Delay(TimeSpan.FromSeconds(6)).ConfigureAwait(false);
                    await ProcessWait(cts.Token).ConfigureAwait(false);

//                    await task.ConfigureAwait(false);
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
            await Task.Delay(5000, ct).ConfigureAwait(false);

            Console.WriteLine("すごく重い処理その1(´・ω・｀)おわり");
        }

        static void HeavyMethod2()
        {
            Console.WriteLine("すごく重い処理その2(´・ω・｀)はじまり");
            Thread.Sleep(3000);
            Console.WriteLine("すごく重い処理その2(´・ω・｀)おわり");
        }

        static async Task<int> ProcessWait(CancellationToken cancellationToken)
        {
            Console.WriteLine("Process Wait処理のはじまり");

            var startInfo = new ProcessStartInfo()
            { 
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = false,
                RedirectStandardOutput = false,
                RedirectStandardInput = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = "127.0.0.1 -n 10",
                FileName = "ping",
            };

            using (var process = new Process()
            {
                StartInfo = startInfo,
            })
            {
                process.Start();
                var result = await process.WaitForExitAsync(cancellationToken).ConfigureAwait(false);
                Console.WriteLine("Process Wait処理のおわり");

                return result;
            }
        }
    }
}
