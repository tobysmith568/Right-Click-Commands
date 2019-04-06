using Right_Click_Commands.Models;
using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
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

        private readonly IContextMenuWorker registryWorker = new RegistryWorker();//TODO DI
        private readonly ISettings settings = new WindowsSettings();//TODO DI

        private ObservableCollection<IScriptConfig> scriptConfigs;
        private IScriptConfig selectedScriptConfig;

        //  Properties
        //  ==========

        public ObservableCollection<IScriptConfig> ScriptConfigs
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

        public IScriptConfig SelectedScriptConfig
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
            ScriptConfigs = new ObservableCollection<IScriptConfig>(registryWorker.GetScriptConfigs());
            
            SimpleCommand = new Command(DoSimpleCommand);
        }

        //  Methods
        //  =======

        private void DoSimpleCommand()
        {
            settings.SaveAll();
        }
    }
}
