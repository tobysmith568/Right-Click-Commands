using Right_Click_Commands.Models.ContextMenu;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.Models.Updater;
using System;
using IconPicker;
using Right_Click_Commands.Models.Dialogs.SaveFile;

namespace Right_Click_Commands.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        //  Variables
        //  =========

        private readonly IContextMenuWorker contextMenuWorker;
        private readonly IScriptFactory<IScriptStorageModel> scriptFactory;
        private readonly ISettings settings;
        private readonly IMessagePrompt messagePrompt;
        private readonly IUpdater updater;
        private readonly IIconPicker iconPicker;
        private readonly ISaveFileDialog saveFileDialog;

        private ScriptCollection scriptConfigs;
        private IScriptConfig selectedScriptConfig;
        private int selectedScriptConfigIndex;

        //  Properties
        //  ==========

        public ScriptCollection ScriptConfigs
        {
            get => scriptConfigs;
            set => PropertyChanging(value, ref scriptConfigs, nameof(ScriptConfigs));
        }

        public IScriptConfig SelectedScriptConfig
        {
            get => selectedScriptConfig;
            set => PropertyChanging(value, ref selectedScriptConfig, nameof(SelectedScriptConfig), nameof(SelectedScriptConfigIndex));
        }

        public int SelectedScriptConfigIndex
        {
            get => selectedScriptConfigIndex;
            set => PropertyChanging(value, ref selectedScriptConfigIndex, nameof(SelectedScriptConfig), nameof(SelectedScriptConfigIndex));
        }

        public Command ViewFullyLoaded { get; }
        public Command WindowCloseCommand { get; }
        public Command<string> CreateNewScript { get; }
        public Command MoveSelectedUp { get; }
        public Command MoveSelectedDown { get; }
        public Command DeleteSelected { get; }
        public Command SelectNewIcon { get; }
        public Command SaveAs { get; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {
            ViewFullyLoaded = new Command(DoViewFullyLoaded);
            WindowCloseCommand = new Command(DoWindowCloseCommand);
            CreateNewScript = new Command<string>(DoCreateNewScript);
            MoveSelectedUp = new Command(DoMoveSelectedUp);
            MoveSelectedDown = new Command(DoMoveSelectedDown);
            DeleteSelected = new Command(DoDeleteSelected);
            SelectNewIcon = new Command(DoSelectNewIcon);
            SaveAs = new Command(DoSaveAs);
        }

        public MainWindowViewModel(IContextMenuWorker contextMenuWorker,
                                   IScriptFactory<IScriptStorageModel> scriptFactory,
                                   ISettings settings,
                                   IMessagePrompt messagePrompt,
                                   IUpdater updater,
                                   IIconPicker iconPicker,
                                   ISaveFileDialog saveFileDialog) : this()
        {
            this.contextMenuWorker = contextMenuWorker ?? throw new ArgumentNullException(nameof(contextMenuWorker));
            this.scriptFactory = scriptFactory ?? throw new ArgumentNullException(nameof(scriptFactory));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.messagePrompt = messagePrompt ?? throw new ArgumentNullException(nameof(messagePrompt));
            this.updater = updater ?? throw new ArgumentNullException(nameof(updater));
            this.iconPicker = iconPicker ?? throw new ArgumentNullException(nameof(iconPicker));
            this.saveFileDialog = saveFileDialog ?? throw new ArgumentNullException(nameof(saveFileDialog));

            selectedScriptConfigIndex = -1;

            ScriptConfigs = new ScriptCollection(this.contextMenuWorker.GetScriptConfigs());
        }

        //  Methods
        //  =======

        private async void DoViewFullyLoaded()
        {
            Asset asset = await updater.CheckForUpdateAsync();

            if (asset != null)
            {
                updater.UpdateTo(asset);
            }
        }

        private void DoWindowCloseCommand()
        {
            settings.SaveAll();

            foreach (IScriptConfig scriptConfig in ScriptConfigs)
            {
                scriptConfig.SaveScript();
            }

            contextMenuWorker.SaveScriptConfigs(ScriptConfigs);
        }

        private void DoCreateNewScript(string scriptType)
        {
            try
            {
                scriptConfigs.Add(scriptFactory.Generate(scriptType, scriptConfigs.Count.ToString("D2")));
                SelectedScriptConfigIndex = ScriptConfigs.Count - 1;
            }
            catch
            {
                messagePrompt.PromptOK($"Unable to create new script of type [{scriptType}]!", "Error creating new script", MessageType.Error);
            }
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

        private void DoSelectNewIcon()
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

            if (scriptConfigs[selectedScriptConfigIndex].Icon != null)
            {
                switch(messagePrompt.PromptYesNo($"Are you sure you want to remove the icon from {scriptConfigs[selectedScriptConfigIndex].Label}?", "Are you sure?", MessageType.Warning))
                {
                    case MessageResult.Yes:
                        scriptConfigs[selectedScriptConfigIndex].Icon = null;
                        RaisePropertyChanged(nameof(SelectedScriptConfig));
                        RaisePropertyChanged(nameof(SelectedScriptConfigIndex));
                        return;
                    default:
                        return;
                }
            }

            IIconReference iconReference = iconPicker.SelectIconReference();

            if (iconReference == null)
            {
                return;
            }

            scriptConfigs[selectedScriptConfigIndex].Icon = iconReference;
            RaisePropertyChanged(nameof(SelectedScriptConfig));
            RaisePropertyChanged(nameof(SelectedScriptConfigIndex));
        }

        public void DoSaveAs()
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

            saveFileDialog.Save(scriptConfigs[selectedScriptConfigIndex].Script, scriptConfigs[selectedScriptConfigIndex].FileExtension, scriptConfigs[selectedScriptConfigIndex].ScriptType, scriptConfigs[selectedScriptConfigIndex].Label);
        }
    }
}