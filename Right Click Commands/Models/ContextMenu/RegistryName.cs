namespace Right_Click_Commands.Models.ContextMenu
{
    public struct RegistryName
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public RegistryName(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
