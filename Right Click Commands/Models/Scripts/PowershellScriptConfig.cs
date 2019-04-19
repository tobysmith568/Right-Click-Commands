using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public class PowershellScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        private const string dotPS = ".ps";

        //  Properties
        //  ==========

        public override string ScriptType => "Powershell Script";

        //  Constructors
        //  ============

        public PowershellScriptConfig(string name, string id) : base(name, id)
        {
            ScriptLocation = Path.Combine(appDataFolder, Name + dotPS);
        }
    }
}
