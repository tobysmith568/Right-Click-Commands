using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Scripts
{
    public class BatScriptConfig : ViewModelBase, IScriptConfig
    {
        //  Variables
        //  =========

        private static readonly string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Right-Click Commands");

        private string label = "";
        private string icon;
        private string script;
        private bool onDirectory;
        private bool onBackground;

        //  Properties
        //  ==========

        public string Name { get; }

        public string Label
        {
            get => label;
            set => PropertyChanging(value, ref label, "Label");
        }

        public string Icon
        {
            get => icon;
            set => PropertyChanging(value, ref icon, "Icon");
        }

        public string Script
        {
            get => script;
            set => PropertyChanging(value, ref script, "Script");
        }

        public bool OnDirectory
        {
            get => onDirectory;
            set => PropertyChanging(value, ref onDirectory, "OnDirectory");
        }

        public bool OnBackground
        {
            get => onBackground;
            set => PropertyChanging(value, ref onBackground, "OnBackground");
        }

        //  Constructors
        //  ============

        public BatScriptConfig(string name)
        {
            Name = name;
            LoadScript();
        }

        //  Methods
        //  =======

        private void LoadScript()
        {
            try
            {
                string scriptFile = Path.Combine(appDataFolder, Name + ".bat");

                if (!Directory.Exists(appDataFolder))
                {
                    Directory.CreateDirectory(appDataFolder);
                }

                if (!File.Exists(scriptFile))
                {
                    File.WriteAllText(scriptFile, "");
                }

                Script = File.Exists(scriptFile) ? File.ReadAllText(scriptFile) : "";
            }
            catch // TODO
            {

            }
        }

        public void SaveScript()
        {
            try
            {
                string scriptFile = Path.Combine(appDataFolder, Name + ".bat");

                if (!Directory.Exists(appDataFolder))
                {
                    Directory.CreateDirectory(appDataFolder);
                }

                File.WriteAllText(scriptFile, Script);
            }
            catch // TODO
            {

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