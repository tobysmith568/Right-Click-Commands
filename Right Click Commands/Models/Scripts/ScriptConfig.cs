namespace Right_Click_Commands.Models.Scripts
{
    public abstract class ScriptConfig : ViewModelBase, IScriptConfig
    {
        //  Properties
        //  ==========

        public abstract string ScriptType { get; }
        public abstract string ID { get; set; }
        public abstract string Name { get; }
        public abstract string ScriptLocation { get; protected set; }
        public abstract string Label { get; set; }
        public abstract string Script { get; set; }
        public abstract bool OnDirectory { get; set; }
        public abstract bool OnBackground { get; set; }
        public abstract bool KeepWindowOpen { get; set; }

        //  Methods
        //  =======

        /// <exception cref="ScriptAccessException"></exception>
        public abstract void LoadScript();
        /// <exception cref="ScriptAccessException"></exception>
        public abstract void SaveScript();
        public abstract void ModifyLocation(MenuLocation location, bool enabled);
        public abstract bool IsForLocation(MenuLocation location);
    }
}
