using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Runner
{
    public class ExecutionException : Exception
    {
        //  Constructors
        //  ============

        public ExecutionException(string message = "", Exception e = null) : base(message, e)
        {

        }
    }
}
