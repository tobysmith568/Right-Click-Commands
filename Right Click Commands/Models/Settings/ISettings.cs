namespace Right_Click_Commands.Models.Settings
{
    public interface ISettings
    {
        //  Properties
        //  ==========

        string ScriptLocation { get; }
        bool JustInstalled { get; set; }

        //  Methods
        //  =======

        void Upgrade();
        void SaveAll();
    }
}
