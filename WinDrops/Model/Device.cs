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
using System.Collections.ObjectModel;
using WinDrops.Utils;

namespace WinDrops.Model
{
    [Table("Device")]
    public class Device : ViewModelBase
    {
        public enum EDevType
        {
            undef,
            Valve,
            Flash,
            Camera
        }

        private string _Name;
        private int _Pin;
        private int _Duration;
        private ObservableCollection<DropTask> _DropTasks;

        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("DropConfigId")]
        public int DropConfigId { get; set; }

        [Column("Name")]
        public string Name
        {
            get => _Name;
            set { SetProperty(ref _Name, value); }
        }

        [Column("Pin")]
        public int Pin
        {
            get => _Pin;
            set { SetProperty(ref _Pin, value); }
        }

        [Column("Duration")]
        public int Duration
        {
            get => _Duration;
            set { SetProperty(ref _Duration, value); }
        }

        [Column("DevType")]
        public EDevType DevType { get; set; }

        [Ignore]
        public ObservableCollection<DropTask> DropTasks
        {
            get => _DropTasks;
            set { SetProperty(ref _DropTasks, value); }
        }

        public Device()
            : this(-1, EDevType.undef, "", -1, -1, new ObservableCollection<DropTask>()) { }

        public Device(int dropConfigId)
            : this(dropConfigId, EDevType.undef, "", -1, -1, new ObservableCollection<DropTask>()) { }

        public Device(int dropConfigId, EDevType devType)
            : this(dropConfigId, devType, "", -1, -1, new ObservableCollection<DropTask>()) { }

        public Device(int dropConfigId, EDevType devType, string name, int pin, int duration)
            : this(dropConfigId, devType, name, pin, duration, new ObservableCollection<DropTask>()) { }

        public Device(int dropConfigId, EDevType devType, string name, int pin, int duration, ObservableCollection<DropTask> dropTasks)
        {
            DropConfigId = dropConfigId;
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
