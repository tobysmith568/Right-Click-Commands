using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Right_Click_Commands.Models.MessagePrompts
{
    public class WinDialogBox : IMessagePrompt
    {
        public void TellInformation(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        public void TellWarning(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
        }

        public void TellError(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        public MessageResult PromptOKCancel(string message, string title)
        {
            switch (MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK))
            {
                case MessageBoxResult.OK:
                    return MessageResult.OK;
                default:
                    return MessageResult.Cancel;
            }
        }

        public MessageResult PromptYesNo(string message, string title)
        {
            switch (MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.Yes))
            {
                case MessageBoxResult.Yes:
                    return MessageResult.Yes;
                default:
                    return MessageResult.No;
            }
        }

        public MessageResult PromptYesNoCancel(string message, string title)
        {
            switch (MessageBox.Show(message, title, MessageBoxButton.YesNoCancel, MessageBoxImage.None, MessageBoxResult.Yes))
            {
                case MessageBoxResult.Yes:
                    return MessageResult.Yes;
                case MessageBoxResult.No:
                    return MessageResult.No;
                default:
                    return MessageResult.Cancel;
            }
        }
    }
}
