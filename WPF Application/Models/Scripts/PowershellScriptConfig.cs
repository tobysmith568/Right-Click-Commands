using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Scripts
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

        public PowershellScriptConfig(string name, string id, ISettings settings) : base(name, id, settings)
        {
            ScriptLocation = Path.Combine(settings.ScriptLocation, Name + dotPS);
        }
    }
}
