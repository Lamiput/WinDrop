using SQLite;
using SQLiteNetExtensions.Attributes;


namespace WinDrops.Model
{
    public class DropTask
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Device))]
        public int DeviceId { get; set; }

        public int StartTime { get; set; }

        public int Duration { get; set; }

        public DropTask(int startTime=0, int duration=0)
        {
            StartTime = startTime;
            Duration = duration;
        }
    }

}
