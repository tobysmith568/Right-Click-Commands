using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

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
            set => PropertyChanging(value, ref scriptConfigs, "ScriptConfigs");
        }

        public IScriptConfig SelectedScriptConfig
        {
            get => selectedScriptConfig;
            set => PropertyChanging(value, ref selectedScriptConfig, "SelectedScriptConfig");
        }

        public Command WindowCloseCommand { get; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {
            ScriptConfigs = new ObservableCollection<IScriptConfig>(registryWorker.GetScriptConfigs());

            WindowCloseCommand = new Command(DoWindowCloseCommand);
        }

        //  Methods
        //  =======

        private void DoWindowCloseCommand()
        {
            settings.SaveAll();
            
            foreach (IScriptConfig scriptConfig in scriptConfigs)
            {
                scriptConfig.SaveScript();
            }
        }
    }
}
