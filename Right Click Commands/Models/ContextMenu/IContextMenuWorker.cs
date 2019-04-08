﻿using Right_Click_Commands.Models.Scripts;
using System.Collections.Generic;

namespace Right_Click_Commands.Models.ContextMenu
{
    public interface IContextMenuWorker
    {
        //  Methods
        //  =======

        ICollection<ScriptConfig> GetScriptConfigs();
        void SaveScriptConfigs(ICollection<ScriptConfig> configs);
        ScriptConfig New(string id);
    }
}