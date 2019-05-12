using Right_Click_Commands.Models.Scripts;
using Right_Click_Commands.Models.Settings;
using System.IO;

namespace Right_Click_Commands.WPF.Models.Scripts
{
    public class BatScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        private const string dotBat = ".bat";
        public const string keepCMDOpen = "/K";
        public const string closeCMD = "/C";

        //  Properties
        //  ==========

        public override string ScriptType => "Batch Script";

        public override string ExePath => "cmd";

        public override string ScriptArgs => $"\"{ExePath}\" \"{(KeepWindowOpen ? keepCMDOpen : closeCMD)} TITLE {Label}&|{ScriptLocation}|\"";

        //  Constructors
        //  ============

        public BatScriptConfig(string name, string id, ISettings settings) : base(name, id, settings)
        {
            ScriptLocation = Path.Combine(settings.ScriptLocation, Name + dotBat);
        }
    }
}