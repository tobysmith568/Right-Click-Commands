﻿using System.IO;

namespace Right_Click_Commands.Models.Scripts
{
    public class BatScriptConfig : ScriptConfig
    {
        //  Constants
        //  =========

        private const string dotBat = ".bat";
        private const string keepCMDOpen = "/K";
        private const string closeCMD = "/C";

        //  Properties
        //  ==========

        public override string ScriptType => "Batch Script";

        public override string ExePath => "cmd";

        public override string ScriptArgs => $"\"{ExePath}\" \"{(KeepWindowOpen ? keepCMDOpen : closeCMD)} TITLE {Label}&|{ScriptLocation}|\"";

        //  Constructors
        //  ============

        public BatScriptConfig(string name, string id) : base(name, id)
        {
            ScriptLocation = Path.Combine(appDataFolder, Name + dotBat);
        }
    }
}