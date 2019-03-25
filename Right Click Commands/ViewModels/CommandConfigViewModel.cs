using Microsoft.Win32;
using Right_Click_Commands.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.ViewModels
{
    public class CommandConfigViewModel
    {
        //  Properties
        //  ==========

        public ObservableCollection<CommandConfig> CommandConfigs { get; set; }

        //  Constructors
        //  ============

        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="IOException"></exception>
        public void LoadCommandConfigs()
        {
            CommandConfigs = new ObservableCollection<CommandConfig>
            {
                /*new CommandConfig()
                {
                    Label = "Click me",
                    Commands = "thing1&thing2",
                    Icon = @"D:\Program Files\Notepad++\notepad++.exe",
                    HideWindow = false,
                    MenuLocation = MenuLocation.Both,
                    WindowType = WindowType.CommandPrompt
                },
                new CommandConfig()
                {
                    Label = "Click me also",
                    Commands = "thing1&thing2",
                    Icon = @"D:\Program Files\Notepad++\notepad++.exe",
                    HideWindow = false,
                    MenuLocation = MenuLocation.Both,
                    WindowType = WindowType.CommandPrompt
                },
                new CommandConfig()
                {
                    Label = "Click me also also please",
                    Commands = "thing1&thing2",
                    Icon = @"D:\Program Files\Notepad++\notepad++.exe",
                    HideWindow = false,
                    MenuLocation = MenuLocation.Both,
                    WindowType = WindowType.CommandPrompt
                }*/
            };
            /*using (RegistryKey parentKey = Registry.ClassesRoot.OpenSubKey(@"Directory\shell", true))
            {
                foreach (string innerKey in parentKey.GetSubKeyNames().Where(n => n.StartsWith("RightClickCommand")))
                {
                    CommandConfigs.Add(new CommandConfig(parentKey.OpenSubKey(innerKey, true)));
                }
            }*/
        }
    }
}
