//
//  Project: WinDrop - A GUI for ArduDrop or similar Microcontrollerprojects
//  Copyright (C) 2021 Holger Pasligh
//  
//  This file is part of WinDrop.
//  
//  WinDrop is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  WinDrop is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with WinDrop. If not, see <http://www.gnu.org/licenses/>.
//

using SQLite;
using WinDrops.Utils;


namespace WinDrops.Model
{
    [Table("DropTask")]
    public class DropTask : ViewModelBase
    {
        private int _StartTime;
        private int _Duration;

        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("DeviceId")]
        public int DeviceId { get; set; }

        [Column("StartTime")]
        public int StartTime
        {
            get => _StartTime;
            set { SetProperty(ref _StartTime, value); }
        }

        [Column("Duration")]
        public int Duration
        {
            get => _Duration;
            set { SetProperty(ref _Duration, value); }
        }

        public DropTask() : this(0, 0, 0) { }
        public DropTask(int deviceId, int startTime = 0, int duration = 0)
        {
            DeviceId = deviceId;
            StartTime = startTime;
            Duration = duration;
        }
    }

}
