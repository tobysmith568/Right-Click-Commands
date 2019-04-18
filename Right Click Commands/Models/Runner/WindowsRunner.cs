using Right_Click_Commands.Models.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Runner
{
    public class WindowsRunner : IRunner
    {
        //  Variables
        //  =========

        private readonly static WindowsLogger log = new WindowsLogger(typeof(WindowsRunner));

        //  Events
        //  ======

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Error(e.Data);
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                log.Info(e.Data);
            }
        }

        //  Methods
        //  =======

        /// <exception cref="Right_Click_Commands.Models.Runner.ExecutionException"></exception>
        public async Task Run(string file, string arguements)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (Process process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = file,
                            Arguments = arguements,
                        }
                    })
                    {
                        process.Start();
                        process.WaitForExit();
                    }
                });
            }
            catch (Exception e)
            {
                throw new ExecutionException("Unknown exception from script", e);
            }
        }
        
        /// <exception cref="Right_Click_Commands.Models.Runner.ExecutionException"></exception>
        public async Task RunHidden(string file, string arguements)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (Process process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            CreateNoWindow = false,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            FileName = file,
                            Arguments = arguements,
                        }
                    })
                    {
                        process.OutputDataReceived += Process_OutputDataReceived;
                        process.ErrorDataReceived += Process_ErrorDataReceived;
                        process.Start();
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();
                        process.WaitForExit();
                    }
                });
            }
            catch (Exception e)
            {
                throw new ExecutionException("Unknown exception from script", e);
            }
        }
    }
}
