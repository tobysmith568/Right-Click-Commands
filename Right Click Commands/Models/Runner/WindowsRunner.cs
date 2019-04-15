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
                            WindowStyle = ProcessWindowStyle.Normal,
                            //CreateNoWindow = false,
                            //UseShellExecute = false,
                            //RedirectStandardOutput = true,
                            //RedirectStandardError = true,
                            FileName = file,
                            Arguments = arguements
                        }
                    })
                    {
                        //process.OutputDataReceived += Process_OutputDataReceived;
                        //process.ErrorDataReceived += Process_ErrorDataReceived;
                        process.Start();
                        //process.BeginOutputReadLine();
                        //process.BeginErrorReadLine();

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
