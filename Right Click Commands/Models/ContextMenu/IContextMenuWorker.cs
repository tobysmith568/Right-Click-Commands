using Right_Click_Commands.Models.Scripts;
using System.Collections.Generic;

namespace Right_Click_Commands.Models.ContextMenu
{
    public interface IContextMenuWorker
    {
        //  Methods
        //  =======

        ICollection<IScriptConfig> GetScriptConfigs();
        void SaveScriptConfigs(ICollection<IScriptConfig> configs);
        /// <exception cref="ScriptAccessException"></exception>
        IScriptConfig New(ScriptType scriptType, string id);
    }
}