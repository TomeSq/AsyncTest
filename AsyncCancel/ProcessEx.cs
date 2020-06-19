using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace AsyncCancel
{
    public static class ProcessEx
    {
        public static Task<int> WaitForExitAsync(this Process self, CancellationToken cancellationToken)
        {
            if(self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            var tcs = new TaskCompletionSource<int>();

            self.Exited += (sender, args) =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    tcs.SetResult(self.ExitCode);
                }
            };
            self.EnableRaisingEvents = true;

            cancellationToken.Register(()=>
            {
                try
                {
                    Debug.WriteLine("Kill Start");
                    self.Kill();
                }
                catch(SystemException ex) when(ex is Win32Exception || ex is NotSupportedException || ex is InvalidOperationException)
                {
                    Debug.WriteLine($"Process Kill Exception : {ex.Message}");
                }

                Debug.WriteLine("SetCanceled Start");
                tcs.SetCanceled();
            });

            return tcs.Task;
        }
    }
}
