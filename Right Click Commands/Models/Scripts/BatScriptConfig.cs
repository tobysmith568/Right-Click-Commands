using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public class BatScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        private const string dotBat = ".bat";

        //  Properties
        //  ==========

        public override string ScriptType => "Batch Script";

        //  Constructors
        //  ============

        public BatScriptConfig(string name, string id) : base(name, id)
        {
            ScriptLocation = Path.Combine(appDataFolder, Name + dotBat);
        }
    }
}