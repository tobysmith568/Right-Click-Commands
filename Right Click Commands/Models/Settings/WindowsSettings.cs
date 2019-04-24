using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Settings
{
    public class WindowsSettings : ISettings
    {
        //  Properties
        //  ==========

        public string ScriptLocation { get; }

        //  Constructor
        //  ===========

        public WindowsSettings()
        {
            ScriptLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Right-Click Commands");
        }

        //  Methods
        //  =======

        public void SaveAll()
        {
            Properties.Settings.Default.Save();
        }
    }
}
