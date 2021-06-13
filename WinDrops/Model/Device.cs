using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;


namespace WinDrops.Model
{
    public class Device
    {
        public enum EDevType
        {
            undef,
            Valve,
            Flash,
            Camera
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(DropConfig))]
        public int DropConfigId { get; set; }

        public string Name { get; set; }

        public int Pin { get; set; }

        public int Duration { get; set; }

        public EDevType DevType { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<DropTask> DropTasks { get; set; }

        public Device()
            : this(EDevType.undef, "", -1, -1, new List<DropTask>()) { }

        public Device(EDevType devType)
            : this(devType, "", -1, -1, new List<DropTask>()) { }

        public Device(EDevType devType, string name, int pin, int duration)
            : this(devType, name, pin, duration, new List<DropTask>()) { }

        public Device(EDevType devType, string name, int pin, int duration, List<DropTask> dropTasks)
        {
            DevType = devType;
            Name = name != "" ? name : devType.ToString();
            Pin = pin;
            if (duration < 0)
            {
                switch (devType)
                {
                    case EDevType.Valve:
                        duration = 1;
                        break;
                    case EDevType.Flash:
                        duration = 10;
                        break;
                    case EDevType.Camera:
                        duration = 50;
                        break;
                    default:
                        duration = 0;
                        break;
                }
            }
            Duration = duration;
            DropTasks = dropTasks;
        }
    }
}
