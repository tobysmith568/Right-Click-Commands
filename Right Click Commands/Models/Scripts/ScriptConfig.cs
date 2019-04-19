using System;
using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public abstract class ScriptConfig : ViewModelBase, IScriptConfig
    {
        //  Variables
        //  =========

        protected static readonly string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Right-Click Commands");

        private string label = string.Empty;
        private string icon;
        private string script;
        private bool onDirectory;
        private bool onBackground;
        private bool keepWindowOpen;

        //  Properties
        //  ==========

        public abstract string ScriptType { get; }

        public string ID { get; set; }

        public string Name { get; }

        public string ScriptLocation { get; protected set; }

        public string Label
        {
            get => label;
            set => PropertyChanging(value, ref label, nameof(Label));
        }

        public string Icon
        {
            get => icon;
            set => PropertyChanging(value, ref icon, nameof(Icon));
        }

        public string Script
        {
            get => script;
            set => PropertyChanging(value, ref script, nameof(Script));
        }

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

        public ScriptConfig(string name, string id)
        {
            Name = name;
            ID = id;
        }

        //  Methods
        //  =======

        /// <exception cref="ScriptAccessException"></exception>
        public void LoadScript()
        {
            try
            {
                if (!Directory.Exists(appDataFolder))
                {
                    Directory.CreateDirectory(appDataFolder);
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

        /// <exception cref="ScriptAccessException"></exception>
        public void SaveScript()
        {
            try
            {
                if (!Directory.Exists(appDataFolder))
                {
                    Directory.CreateDirectory(appDataFolder);
                }

                File.WriteAllText(ScriptLocation, Script);
            }
            catch (Exception e)
            {
                throw new ScriptAccessException($"Cannot open the script file for the script [{Name}]", e);
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
