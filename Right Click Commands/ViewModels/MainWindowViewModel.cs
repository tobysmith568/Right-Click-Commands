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

        private readonly IContextMenuWorker contextMenuWorker;
        private readonly ISettings settings;

        private ObservableCollection<ScriptConfig> scriptConfigs;
        private IScriptConfig selectedScriptConfig;
        private int selectedScriptConfigIndex;

        //  Properties
        //  ==========

        public ObservableCollection<ScriptConfig> ScriptConfigs
        {
            get => scriptConfigs;
            set => PropertyChanging(value, ref scriptConfigs, "ScriptConfigs");
        }

        public IScriptConfig SelectedScriptConfig
        {
            get => selectedScriptConfig;
            set => PropertyChanging(value, ref selectedScriptConfig, "SelectedScriptConfig");
        }

        public int SelectedScriptConfigIndex
        {
            get => selectedScriptConfigIndex;
            set => PropertyChanging(value, ref selectedScriptConfigIndex, "SelectedScriptConfigIndex");
        }

        public Command WindowCloseCommand { get; }

        public Command CreateNewScript { get; }

        public Command MoveSelectedUp { get; }

        public Command MoveSelectedDown { get; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IContextMenuWorker contextMenuWorker, ISettings settings) : this()
        {
            this.contextMenuWorker = contextMenuWorker;
            this.settings = settings;

            ScriptConfigs = new ObservableCollection<ScriptConfig>(this.contextMenuWorker.GetScriptConfigs());

            WindowCloseCommand = new Command(DoWindowCloseCommand);
            CreateNewScript = new Command(DoCreateNewScript);
            MoveSelectedUp = new Command(DoMoveSelectedUp);
            MoveSelectedDown = new Command(DoMoveSelectedDown);
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

        private void DoCreateNewScript()
        {
            scriptConfigs.Add(contextMenuWorker.New(scriptConfigs.Count.ToString()));
        }

        private void DoMoveSelectedUp()
        {
            if (SelectedScriptConfigIndex < 1)
            {
                return;
            }

            scriptConfigs.MoveUpOne(SelectedScriptConfigIndex);
        }

        private void DoMoveSelectedDown()
        {
            if (SelectedScriptConfigIndex == -1 || SelectedScriptConfigIndex >= ScriptConfigs.Count - 1)
            {
                return;
            }

            scriptConfigs.MoveDownOne(SelectedScriptConfigIndex);
        }
    }
}