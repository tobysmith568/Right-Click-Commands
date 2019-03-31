using System.Collections.Generic;

namespace Right_Click_Commands.Models
{
    public interface IScriptWorker
    {
        ICollection<ScriptConfig> GetScriptConfigs();
    }
}