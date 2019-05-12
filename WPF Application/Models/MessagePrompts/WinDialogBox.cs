using Right_Click_Commands.Models.MessagePrompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Right_Click_Commands.WPF.Models.MessagePrompts
{
    public class WinDialogBox : IMessagePrompt
    {
        //  Methods
        //  =======

        public void PromptOK(string message, string title = "", MessageType messageType = MessageType.None)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, ConvertImage(messageType), MessageBoxResult.OK);
        }

        public MessageResult PromptOKCancel(string message, string title = "", MessageType messageType = MessageType.None)
        {
            switch (MessageBox.Show(message, title, MessageBoxButton.OKCancel, ConvertImage(messageType), MessageBoxResult.OK))
            {
                case MessageBoxResult.OK:
                    return MessageResult.OK;
                default:
                    return MessageResult.Cancel;
            }
        }

        public MessageResult PromptYesNo(string message, string title = "", MessageType messageType = MessageType.None)
        {
            switch (MessageBox.Show(message, title, MessageBoxButton.YesNo, ConvertImage(messageType), MessageBoxResult.Yes))
            {
                case MessageBoxResult.Yes:
                    return MessageResult.Yes;
                default:
                    return MessageResult.No;
            }
        }

        public MessageResult PromptYesNoCancel(string message, string title = "", MessageType messageType = MessageType.None)
        {
            switch (MessageBox.Show(message, title, MessageBoxButton.YesNoCancel, ConvertImage(messageType), MessageBoxResult.Yes))
            {
                case MessageBoxResult.Yes:
                    return MessageResult.Yes;
                case MessageBoxResult.No:
                    return MessageResult.No;
                default:
                    return MessageResult.Cancel;
            }
        }

        private MessageBoxImage ConvertImage(MessageType type)
        {
            switch (type)
            {
                case MessageType.Info:
                    return MessageBoxImage.Information;
                case MessageType.Warning:
                    return MessageBoxImage.Warning;
                case MessageType.Error:
                    return MessageBoxImage.Error;
                default:
                    return MessageBoxImage.None;
            }
        }
    }
}
