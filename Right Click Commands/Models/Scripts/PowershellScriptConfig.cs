using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public class PowershellScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        private const string dotPS = ".ps1";
        private const string NoExit = "-NoExit";
        private const string Exit = "";

        //  Properties
        //  ==========

        public override string ScriptType => "Powershell Script";

        public override string ExePath => "powershell";

        public override string ScriptArgs => $"\"{ExePath}\" \"{(KeepWindowOpen ? NoExit : Exit)} -ExecutionPolicy Bypass -file |{ScriptLocation}|\"";

        //  Constructors
        //  ============

        public PowershellScriptConfig(string name, string id) : base(name, id)
        {
            ScriptLocation = Path.Combine(appDataFolder, Name + dotPS);
        }
    }
}
