using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Runner
{
    public interface IRunner
    {
        /// <exception cref="Right_Click_Commands.Models.Runner.ExecutionException"></exception>
        Task Run(string file, string arguements);
    }
}
