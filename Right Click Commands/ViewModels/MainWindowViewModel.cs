﻿using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Unity;

namespace Right_Click_Commands.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //  Variables
        //  =========

        private readonly IContextMenuWorker contextMenuWorker;
        private readonly ISettings settings;

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

        }

        public MainWindowViewModel(IContextMenuWorker contextMenuWorker, ISettings settings) : this()
        {
            this.contextMenuWorker = contextMenuWorker;
            this.settings = settings;

            ScriptConfigs = new ObservableCollection<IScriptConfig>(this.contextMenuWorker.GetScriptConfigs());

            WindowCloseCommand = new Command(DoWindowCloseCommand);
        }

        //  Methods
        //  =======

        private void DoWindowCloseCommand()
        {
            settings.SaveAll();

            foreach (IScriptConfig scriptConfig in ScriptConfigs)
            {
                scriptConfig.SaveScript();
            }

            contextMenuWorker.SaveScriptConfigs(ScriptConfigs);
        }
    }
}
