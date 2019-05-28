using IconPicker;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using Right_Click_Commands.ViewModels;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Right_Click_Commands.WPF.Models.Scripts
{
    public abstract class ScriptConfig : ViewModelBase, IScriptConfig
    {
        //  Variables
        //  =========

        protected readonly ISettings settings;
        protected readonly IMessagePrompt messagePrompt;
        protected readonly IIconPicker iconPicker;

        private string label = string.Empty;
        private string script;
        private IIconReference icon;
        private bool onDirectory;
        private bool onBackground;
        private bool keepWindowOpen;

        //  Properties
        //  ==========

        public abstract string ScriptType { get; }

        public abstract string ExePath { get; }

        public abstract string ScriptArgs { get; }

        public string ID { get; set; }

        public string Name { get; }

        public virtual string ScriptLocation { get; protected set; }

        public string Label
        {
            get => label;
            set => PropertyChanging(value, ref label, nameof(Label));
        }

        public string Script
        {
            get => script;
            set => PropertyChanging(value, ref script, nameof(Script));
        }

        public abstract string FileExtension { get; protected set; }

        public abstract string DefaultScript { get; protected set; }

        public IIconReference Icon
        {
            get => icon;
            set
            {
                IconImage = value == null ? null : iconPicker.SelectIconAsBitmap(value);
                PropertyChanging(value, ref icon, nameof(Icon), nameof(IconImage));
            }
        }

        public BitmapSource IconImage { get; private set; }

        public bool OnDirectory
        {
            get => onDirectory;
            set => PropertyChanging(value, ref onDirectory, nameof(OnDirectory));
        }

        public bool OnBackground
        {
            get => onBackground;
            set => PropertyChanging(value, ref onBackground, nameof(OnBackground));
        }

        public bool KeepWindowOpen
        {
            get => keepWindowOpen;
            set => PropertyChanging(value, ref keepWindowOpen, nameof(KeepWindowOpen));
        }

        //  Constructors
        //  ============

        public ScriptConfig(string name, string id, ISettings settings, IMessagePrompt messagePrompt, IIconPicker iconPicker)
        {
            this.settings = settings;
            this.messagePrompt = messagePrompt;
            this.iconPicker = iconPicker;

            Name = name;
            ID = id;

            ScriptLocation = Path.Combine(settings.ScriptLocation, Name + FileExtension);
        }

        //  Methods
        //  =======

        /// <exception cref="ScriptAccessException"></exception>
        public void LoadScript()
        {
            try
            {
                if (!Directory.Exists(settings.ScriptLocation))
                {
                    Directory.CreateDirectory(settings.ScriptLocation);
                }

                if (!File.Exists(ScriptLocation))
                {
                    File.WriteAllText(ScriptLocation, string.Empty);
                }

                Script = File.ReadAllText(ScriptLocation);
            }
            catch (Exception e)
            {
                throw new ScriptAccessException($"Cannot open the script file for the script [{Name}]", e);
            }
        }
        
        public void SaveScript()
        {
            try
            {
                if (!Directory.Exists(settings.ScriptLocation))
                {
                    Directory.CreateDirectory(settings.ScriptLocation);
                }

                File.WriteAllText(ScriptLocation, Script);
            }
            catch
            {
                messagePrompt.PromptOK($"Cannot open the script file for the script [{Label}]", "Error saving script", MessageType.Error);
            }
        }

        public void ModifyLocation(MenuLocation location, bool enabled = true)
        {
            switch (location)
            {
                case MenuLocation.Directory:
                    OnDirectory = enabled;
                    break;
                case MenuLocation.Background:
                    OnBackground = enabled;
                    break;
                case MenuLocation.Both:
                    OnDirectory = enabled;
                    OnBackground = enabled;
                    break;
                default:
                    throw new NotImplementedException($"The given MenuLocation [{location}] has not been implemented");
            }
        }

        public bool IsForLocation(MenuLocation location)
        {
            switch (location)
            {
                case MenuLocation.Directory:
                    return OnDirectory;
                case MenuLocation.Background:
                    return OnBackground;
                case MenuLocation.Both:
                    return OnDirectory && OnBackground;
                default:
                    throw new NotImplementedException($"The given MenuLocation [{location}] has not been implemented");
            }
        }
    }
}
