using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Scripts
{
    public interface IScriptConfig
    {
        //  Properties
        //  ==========

        string Name { get; }
        string ScriptLocation { get; }
        string Label { get; set; }
        string Script { get; set; }
        bool OnDirectory { get; set; }
        bool OnBackground { get; set; }
        bool KeepWindowOpen { get; set; }

        //  Methods
        //  =======

        void LoadScript();
        void SaveScript();
        void ModifyLocation(MenuLocation location, bool enabled);
        bool IsForLocation(MenuLocation location);
    }
}
