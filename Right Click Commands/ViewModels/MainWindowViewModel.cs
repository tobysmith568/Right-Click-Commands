﻿using Right_Click_Commands.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Right_Click_Commands.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        //  Variables
        //  =========

        private ObservableCollection<ScriptConfig> scriptConfigs;
        private ScriptConfig selectedScript;

        //  Properties
        //  ==========

        public ObservableCollection<ScriptConfig> ScriptConfigs
        {
            get => scriptConfigs;
            set
            {
                if (scriptConfigs != value)
                {
                    scriptConfigs = value;
                    RaisePropertyChanged("ScriptConfigs");
                }
            }
        }

        public ScriptConfig SelectedScript
        {
            get => selectedScript;
            set
            {
                if (selectedScript != value)
                {
                    selectedScript = value;
                    RaisePropertyChanged("SelectedScript");
                }
            }
        }

        public Command SimpleCommand { get; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {
            ScriptConfigs = new ObservableCollection<ScriptConfig>()
            {
                new ScriptConfig()
                {
                    Label = "Item #1111111111"
                },
                new ScriptConfig()
                {
                    Label = "Item #2"
                }
            };
            
            SimpleCommand = new Command(DoSimpleCommand);
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

        private void DoSimpleCommand()
        {
            MessageBox.Show("Hello");
        }
    }
}
