using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;


namespace WinDrops.Model
{
    public class DropConfig
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Device> Devices { get; set; }

        public DropConfig() : this("Dropconfig") { }

        public DropConfig(string name) : this(name, "Description") { }

        public DropConfig(string name, string decription) : this(name, decription, new List<Device>()) { }

        public DropConfig(string name, string decription, List<Device> devices)
        {
            Name = name;
            Description = decription;
            Devices = devices;
        }
    }
}
