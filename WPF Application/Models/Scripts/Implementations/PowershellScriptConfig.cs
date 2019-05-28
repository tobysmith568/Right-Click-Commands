using IconPicker;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Scripts
{
    public class PowershellScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        public const string noExit = "-NoExit";
        public const string exit = "";

        //  Properties
        //  ==========

        public override string ScriptType => "Powershell Script";

        public override string ExePath => "powershell";

        public override string ScriptArgs => $"\"{ExePath}\" \"{(KeepWindowOpen ? noExit : exit)} -nologo -ExecutionPolicy Bypass -command |& Set-Location '%v'; & '{ScriptLocation}'|\"";

        public override string FileExtension { get; protected set; } = ".ps1";

        public override string DefaultScript { get; protected set; } = "";

        //  Constructors
        //  ============

        public PowershellScriptConfig(string name, string id, ISettings settings, IMessagePrompt messagePrompt, IIconPicker iconPicker) : base(name, id, settings, messagePrompt, iconPicker)
        {
        }
    }
}
