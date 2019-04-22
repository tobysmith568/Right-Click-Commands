namespace Right_Click_Commands.Models.ContextMenu
{
    public struct RegistryName
    {
        //  Properties
        //  ==========

        public string ID { get; set; }
        public string Name { get; set; }

        //  Constructors
        //  ============

        public RegistryName(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
