using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Scripts
{
    public class ScriptAccessException : Exception
    {
        //  Constructors
        //  ============

        public ScriptAccessException(string message, Exception inner = null) : base(message, inner)
        {
        }
    }
}
