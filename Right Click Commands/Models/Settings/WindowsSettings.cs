using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Settings
{
    public class WindowsSettings : ISettings
    {
        //  Methods
        //  =======

        public void SaveAll()
        {
            Properties.Settings.Default.Save();
        }
    }
}
