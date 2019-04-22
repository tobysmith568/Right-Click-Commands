namespace Right_Click_Commands.Models.Scripts
{
    public interface IScriptConfig
    {
        //  Properties
        //  ==========

        string ScriptType { get; }
        string ExePath { get; }
        string ScriptArgs { get; }

        string ID { get; set; }
        string Name { get; }
        string ScriptLocation { get; }
        string Label { get; set; }
        string Script { get; set; }
        bool OnDirectory { get; set; }
        bool OnBackground { get; set; }
        bool KeepWindowOpen { get; set; }

        //  Methods
        //  =======

        /// <exception cref="ScriptAccessException"></exception>
        void LoadScript();
        /// <exception cref="ScriptAccessException"></exception>
        void SaveScript();
        void ModifyLocation(MenuLocation location, bool enabled);
        bool IsForLocation(MenuLocation location);
    }
}
