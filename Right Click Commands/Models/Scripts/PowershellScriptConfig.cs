using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public class PowershellScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        private const string dotPS = ".ps1";
        public const string noExit = "-NoExit";
        private const string exit = "";

        //  Properties
        //  ==========

        public override string ScriptType => "Powershell Script";

        public override string ExePath => "powershell";

        public override string ScriptArgs => $"\"{ExePath}\" \"{(KeepWindowOpen ? noExit : exit)} -ExecutionPolicy Bypass -file |{ScriptLocation}|\"";

        //  Constructors
        //  ============

        public PowershellScriptConfig(string name, string id) : base(name, id)
        {
            ScriptLocation = Path.Combine(appDataFolder, Name + dotPS);
        }
    }
}
