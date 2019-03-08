using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace Right_Click_Commands.Model
{
    public class CommandConfig : INotifyPropertyChanged
    {
        //  Variables
        //  =========

        private string label = "";
        private string icon;
        private string commands = "";
        private bool hideWindow = false;
        private MenuLocation menuLocation = MenuLocation.Both;
        private WindowType windowType = WindowType.CommandPrompt;

        //  Properties
        //  ==========
        
        public string Label
        {
            get => label;
            set
            {
                if (label != value)
                {
                    label = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        
        public string Icon
        {
            get => icon;
            set
            {
                if (icon != value)
                {
                    icon = value;
                    RaisePropertyChanged("Icon");
                }
            }
        }
        
        public string Commands
        {
            get => commands;
            set
            {
                if (commands != value)
                {
                    commands = value;
                    RaisePropertyChanged("Commands");
                }
            }
        }
        
        public bool HideWindow
        {
            get => hideWindow;
            set
            {
                if (hideWindow != value)
                {
                    hideWindow = value;
                    RaisePropertyChanged("HideWindow");
                }
            }
        }
        
        public MenuLocation MenuLocation
        {
            get => menuLocation;
            set
            {
                if (menuLocation != value)
                {
                    menuLocation = value;
                    RaisePropertyChanged("MenuLocation");
                }
            }
        }
        
        public WindowType WindowType
        {
            get => windowType;
            set
            {
                if (windowType != value)
                {
                    windowType = value;
                    RaisePropertyChanged("WindowType");
                }
            }
        }

        //  Constructors
        //  ============

        public CommandConfig()
        {

        }

        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public CommandConfig(RegistryKey registryKey)
        {
            Label = registryKey.GetValue("MUIVerb", "").ToString();
            Icon = registryKey.GetValue("Icon", null).ToString();
            Commands = GetCommands(registryKey);
        }

        //  Events
        //  ======

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        //  Methods
        //  =======

        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="System.IO.IOException"></exception>
        private string GetCommands(RegistryKey registryKey)
        {
            if (!registryKey.GetSubKeyNames().Contains("command"))
                return "";
            
            using (RegistryKey commandKey = registryKey.OpenSubKey("command", true))
            {
                return commandKey.GetValue("", "").ToString();
            }
        }
    }
}
