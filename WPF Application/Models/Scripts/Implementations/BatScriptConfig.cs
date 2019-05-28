using IconPicker;
using Right_Click_Commands.Models.MessagePrompts;
using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Scripts
{
    public class BatScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        public const string keepCMDOpen = "/K";
        public const string closeCMD = "/C";

        //  Properties
        //  ==========

        public override string ScriptType => "Batch Script";

        public override string ExePath => "cmd";

        public override string ScriptArgs => $"\"{ExePath}\" \"{(KeepWindowOpen ? keepCMDOpen : closeCMD)} TITLE {Label}&cd %v&|{ScriptLocation}|\"";

        public override string FileExtension { get; protected set; } = ".bat";

        public override string DefaultScript { get; protected set; } = "";

        //  Constructors
        //  ============

        public BatScriptConfig(string name, string id, ISettings settings, IMessagePrompt messagePrompt, IIconPicker iconPicker) : base(name, id, settings, messagePrompt, iconPicker)
        {
        }
    }
}