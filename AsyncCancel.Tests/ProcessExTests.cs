using System;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace AsyncCancel.Tests
{
    public class ProcessExTests
    {
        [Fact]
        public async void Procesタイムアウトキャンセル()
        {
            //Arrange
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

            //Act & Assert
            using (var cts = new CancellationTokenSource())
            using (var process = new Process()
            {
                StartInfo = startInfo,
            })
            {
                cts.CancelAfter(TimeSpan.FromSeconds(5));
                process.Start();
                try
                {
                    _ = await process.WaitForExitAsync(cts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                Assert.True(false, "OperationCanceledExceptionの例外が投げられませんでした。");
            }
        }
    }
}