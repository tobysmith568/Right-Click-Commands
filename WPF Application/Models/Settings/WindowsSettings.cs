using Right_Click_Commands.Models.Settings;
using System;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Settings
{
    public class WindowsSettings : ISettings
    {
        //  Properties
        //  ==========

        public string ScriptLocation { get; }

        public bool JustInstalled
        {
            get => Properties.Settings.Default.JustInstalled;
            set => Properties.Settings.Default.JustInstalled = value;
        }

        //  Constructor
        //  ===========

        public WindowsSettings()
        {
            ScriptLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Right-Click Commands");
        }

        //  Methods
        //  =======

        public void Upgrade()
        {
            Properties.Settings.Default.Upgrade();
        }

        public void SaveAll()
        {
            Properties.Settings.Default.Save();
        }
    }
}
