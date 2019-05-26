namespace Right_Click_Commands.Models.Scripts
{
    public interface IScriptStorageModel
    {
        string Name { get; set; }
        string Label { get; set; }
        string Icon { get; set; }
        string Command { get; set; }
    }
}
