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
    [Table("DropConfig")]
    public class DropConfig : ViewModelBase
    {
        private string _Name;
        private string _Description;
        private ObservableCollection<Device> _Devices;


        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name")]
        public string Name
        {
            get => _Name;
            set { SetProperty(ref _Name, value); }
        }

        [Column("Description")]
        public string Description
        {
            get => _Description;
            set { SetProperty(ref _Description, value); }
        }

        [Ignore]
        public ObservableCollection<Device> Devices
        {
            get => _Devices;
            set { SetProperty(ref _Devices, value); }
        }

        public DropConfig() : this("Dropconfig") { }

        public DropConfig(string name) : this(name, "Description") { }

        public DropConfig(string name, string decription) : this(name, decription, new ObservableCollection<Device>()) { }

        public DropConfig(string name, string decription, ObservableCollection<Device> devices)
        {
            Name = name;
            Description = decription;
            Devices = devices;
        }
    }
}
