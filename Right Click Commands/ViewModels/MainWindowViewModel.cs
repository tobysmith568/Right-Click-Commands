using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using System.Collections.ObjectModel;

namespace Right_Click_Commands.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //  Variables
        //  =========

        private readonly IContextMenuWorker contextMenuWorker;
        private readonly ISettings settings;
        private readonly IMessagePrompt messagePrompt;

        private ObservableCollection<ScriptConfig> scriptConfigs;
        private IScriptConfig selectedScriptConfig;
        private int selectedScriptConfigIndex;

        //  Properties
        //  ==========

        public ObservableCollection<ScriptConfig> ScriptConfigs
        {
            get => scriptConfigs;
            set => PropertyChanging(value, ref scriptConfigs, nameof(ScriptConfigs));
        }

        public IScriptConfig SelectedScriptConfig
        {
            get => selectedScriptConfig;
            set => PropertyChanging(value, ref selectedScriptConfig, nameof(SelectedScriptConfig));
        }

        public int SelectedScriptConfigIndex
        {
            get => selectedScriptConfigIndex;
            set => PropertyChanging(value, ref selectedScriptConfigIndex, nameof(SelectedScriptConfigIndex));
        }

        public Command WindowCloseCommand { get; }

        public Command<ScriptType> CreateNewScript { get; }

        public Command MoveSelectedUp { get; }

        public Command MoveSelectedDown { get; }

        public Command DeleteSelected { get; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {

        }

        public MainWindowViewModel(IContextMenuWorker contextMenuWorker, ISettings settings, IMessagePrompt messagePrompt) : this()
        {
            this.contextMenuWorker = contextMenuWorker;
            this.settings = settings;
            this.messagePrompt = messagePrompt;

            selectedScriptConfigIndex = -1;

            ScriptConfigs = new ObservableCollection<ScriptConfig>(this.contextMenuWorker.GetScriptConfigs());

            WindowCloseCommand = new Command(DoWindowCloseCommand);
            CreateNewScript = new Command<ScriptType>(DoCreateNewScript);
            MoveSelectedUp = new Command(DoMoveSelectedUp);
            MoveSelectedDown = new Command(DoMoveSelectedDown);
            DeleteSelected = new Command(DoDeleteSelected);
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

        private void DoCreateNewScript(ScriptType scriptType)
        {
            scriptConfigs.Add(contextMenuWorker.New(scriptType, scriptConfigs.Count.ToString("D2")));

            SelectedScriptConfigIndex = ScriptConfigs.Count - 1;
        }

        private void DoMoveSelectedUp()
        {
            if (SelectedScriptConfigIndex == -1 || SelectedScriptConfigIndex == 0)
            {
                return;
            }

            if (SelectedScriptConfigIndex < -1 || SelectedScriptConfigIndex >= ScriptConfigs.Count)
            {
                SelectedScriptConfigIndex = -1;
                return;
            }

            int selectedindex = SelectedScriptConfigIndex;

            scriptConfigs.MoveUpOne(SelectedScriptConfigIndex);

            SelectedScriptConfigIndex = selectedindex - 1;
        }

        private void DoMoveSelectedDown()
        {
            if (SelectedScriptConfigIndex == -1)
            {
                return;
            }

            if (SelectedScriptConfigIndex < -1 || SelectedScriptConfigIndex >= ScriptConfigs.Count - 1)
            {
                SelectedScriptConfigIndex = -1;
                return;
            }

            int selectedindex = SelectedScriptConfigIndex;

            scriptConfigs.MoveDownOne(SelectedScriptConfigIndex);

            SelectedScriptConfigIndex = selectedindex + 1;
        }

        private void DoDeleteSelected()
        {
            if (SelectedScriptConfigIndex == -1)
            {
                return;
            }

            if (SelectedScriptConfigIndex < -1 || SelectedScriptConfigIndex >= ScriptConfigs.Count)
            {
                SelectedScriptConfigIndex = -1;
                return;
            }

            if (MessageResult.No == messagePrompt.PromptYesNo("Are you sure you want to delete the selected script?", "Are you sure?", MessageType.Warning))
            {
                return;
            }

            int selectedindex = SelectedScriptConfigIndex;

            scriptConfigs.DeleteAtIndex(SelectedScriptConfigIndex);

            if (selectedindex > ScriptConfigs.Count - 1)
            {
                selectedindex = ScriptConfigs.Count - 1;
            }

            SelectedScriptConfigIndex = selectedindex;
        }
    }
}