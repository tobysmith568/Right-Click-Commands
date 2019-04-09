using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.MessagePrompts
{
    public interface IMessagePrompt
    {
        void TellInformation(string message, string title);
        void TellWarning(string message, string title);
        void TellError(string message, string title);
        
        MessageResult PromptOKCancel(string message, string title);
        MessageResult PromptYesNo(string message, string title);
        MessageResult PromptYesNoCancel(string message, string title);
    }

    public enum MessageResult
    {
        Yes,
        No,
        OK,
        Cancel
    }
}
