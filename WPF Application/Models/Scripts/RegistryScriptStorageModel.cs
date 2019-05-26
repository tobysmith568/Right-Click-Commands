using Right_Click_Commands.Models.Scripts;

namespace Right_Click_Commands.WPF.Models.Scripts
{
    public class RegistryScriptStorageModel : IScriptStorageModel
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Command { get; set; }
    }
}
