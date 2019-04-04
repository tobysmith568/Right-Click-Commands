using Right_Click_Commands.Models;
using Right_Click_Commands.Views;
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
    public class MainWindowViewModel : ViewModelBase
    {
        //  Variables
        //  =========

        private readonly RegistryWorker registryWorker = new RegistryWorker();//TODO DI

        private ObservableCollection<ScriptConfig> scriptConfigs;
        private ScriptConfig selectedScriptConfig;

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

        public ScriptConfig SelectedScriptConfig
        {
            get => selectedScriptConfig;
            set
            {
                if (selectedScriptConfig != value)
                {
                    selectedScriptConfig = value;
                    RaisePropertyChanged("SelectedScriptConfig");
                }
            }
        }

        public Command SimpleCommand { get; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {
            ScriptConfigs = new ObservableCollection<ScriptConfig>(registryWorker.GetScriptConfigs());
            
            SimpleCommand = new Command(DoSimpleCommand);
        }

        //  Methods
        //  =======

        private void DoSimpleCommand()
        {
            MessageBox.Show("Hello");
        }
    }
}
