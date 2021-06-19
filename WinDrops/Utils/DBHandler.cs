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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinDrops.Model;


namespace WinDrops.Utils
{
    static class DBHandler
    {
        private static SQLiteConnection _DB;

        public static void OpenConnection()
        {
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WinDropsDB.db");
            _DB = new SQLiteConnection(databasePath);

            _DB.CreateTable<DropConfig>();
            _DB.CreateTable<Device>();
            _DB.CreateTable<DropTask>();
        }

        public static void CloseConnection()
        {
            if (_DB != null)
                _DB.Close();
        }

        public static ObservableCollection<DropConfig> GetDropConfigs()
        {
            List<DropConfig> dropConfigs = _DB.Table<DropConfig>().ToList();
            return new ObservableCollection<DropConfig>(dropConfigs);
        }

        public static DropConfig GetDropConfig(int id)
        {
            DropConfig dropConfig = _DB.Table<DropConfig>().Where(x => x.Id == id).FirstOrDefault();
            if (dropConfig is null)
                return dropConfig;
            List<Device> devices = _DB.Table<Device>().Where(x => x.DropConfigId == dropConfig.Id).ToList();
            if (devices is null)
            { return dropConfig; }
            dropConfig.Devices = new ObservableCollection<Device>(devices);
            for (int i = 0; i < dropConfig.Devices.Count(); i++)
            {
                int tmpDevId = dropConfig.Devices[i].Id;
                List<DropTask> dropTasks = _DB.Table<DropTask>().Where(x => x.DeviceId == tmpDevId).ToList();
                if (dropTasks != null)
                { dropConfig.Devices[i].DropTasks = new ObservableCollection<DropTask>(dropTasks); }
            }
            return dropConfig;
        }

        public static int AddDropConfig(DropConfig dropConfig)
        {
            _ = _DB.Insert(dropConfig);
            return _DB.Table<DropConfig>().Last().Id;
        }

        public static int AddDevice(Device device)
        {
            _ = _DB.Insert(device);
            return _DB.Table<Device>().Where(x => x.DropConfigId == device.DropConfigId).Last().Id;
        }

        public static int AddDropTask(DropTask dropTask)
        {
            _ = _DB.Insert(dropTask);
            return _DB.Table<DropTask>().Where(x => x.DeviceId == dropTask.DeviceId).Last().Id;
        }

        public static int DelDropTask(int id)
        {
            return _DB.Delete<DropTask>(id);
        }

        public static int DelDevice(int id)
        {
            List<DropTask> tasksToDelete = _DB.Table<DropTask>().Where(x => x.DeviceId == id).ToList();
            foreach (DropTask tmpTask in tasksToDelete)
            { _ = _DB.Delete<DropTask>(tmpTask.Id); }
            return _DB.Delete<Device>(id);
        }

        public static int DeleteDropConfig(int id)
        {
            if (id < 1)
            { return 1; }
            List<int> deviceIds = _DB.Table<Device>().Where(x => x.DropConfigId == id).Select(x => x.Id).ToList();
            List<int> dropTaskIds;
            foreach (int deviceId in deviceIds)
            {
                dropTaskIds = _DB.Table<DropTask>().Where(x => x.DeviceId == deviceId).Select(x => x.Id).ToList();
                foreach (int dropTaskId in dropTaskIds)
                {
                    _DB.Delete<DropTask>(dropTaskId);
                }
                _DB.Delete<Device>(deviceId);
            }
            _DB.Delete<DropConfig>(id);
            return 0;
        }

        public static int UpdateDropConfig(DropConfig dropConfig)
        {
            if (dropConfig == null)
            { return 1; }
            if (dropConfig.Id == 0)
            { return 2; }
            _DB.Update(dropConfig);
            foreach (Device device in dropConfig.Devices)
            {
                if (device.Id == 0)
                { continue; }
                _DB.Update(device);
                foreach (DropTask dropTask in device.DropTasks)
                {
                    if (dropTask.Id == 0)
                    { continue; }
                    _DB.Update(dropTask);
                }
            }
            return 0;
        }
    }
}
