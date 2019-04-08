using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Scripts
{
    public class BatScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        private const string dotBat = ".bat";

        //  Variables
        //  =========

        private static readonly string appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Right-Click Commands");
        
        private string label = string.Empty;
        private string icon;
        private string script;
        private bool onDirectory;
        private bool onBackground;
        private bool keepWindowOpen;

        //  Properties
        //  ==========

        public override string ID { get; set; }

        public override string Name { get; }

        public override string ScriptLocation { get; protected set; }

        public override string Label
        {
            get => label;
            set => PropertyChanging(value, ref label, "Label");
        }

        public string Icon
        {
            get => icon;
            set => PropertyChanging(value, ref icon, "Icon");
        }

        public override string Script
        {
            get => script;
            set => PropertyChanging(value, ref script, "Script");
        }

        public override bool OnDirectory
        {
            get => onDirectory;
            set => PropertyChanging(value, ref onDirectory, "OnDirectory");
        }

        public override bool OnBackground
        {
            get => onBackground;
            set => PropertyChanging(value, ref onBackground, "OnBackground");
        }

        public override bool KeepWindowOpen
        {
            get => keepWindowOpen;
            set => PropertyChanging(value, ref keepWindowOpen, "KeepWindowOpen");
        }

        //  Constructors
        //  ============

        public BatScriptConfig(string name, string id)
        {
            Name = name;
            ID = id;
            ScriptLocation = Path.Combine(appDataFolder, Name + dotBat);
            LoadScript();
        }

        //  Methods
        //  =======

        public override void LoadScript()
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

                Script = File.Exists(ScriptLocation) ? File.ReadAllText(ScriptLocation) : string.Empty;
            }
            catch // TODO
            {

            }
        }

        public override void SaveScript()
        {
            try
            {
                string scriptFile = Path.Combine(appDataFolder, Name + dotBat);

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

        public override void ModifyLocation(MenuLocation location, bool enabled = true)
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

        public override bool IsForLocation(MenuLocation location)
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