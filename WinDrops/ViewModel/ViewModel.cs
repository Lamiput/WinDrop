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

using WinDrops.Utils;
using WinDrops.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WinDrops.View;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using System;

namespace WinDrops.ViewModel
{
    class ViewModel : ViewModelBase
    {
        private ObservableCollection<DropConfig> _DropConfigs;
        private DropConfig _ActDropConfig;
        private List<string> _PortList;

        private string _SerialDialog = "";
        private int _Passes = 0;
        private int _Delay = 0;
        private int _SelTabIdx = 0;

        public ObservableCollection<DropConfig> DropConfigs
        {
            get => _DropConfigs;
            set { SetProperty(ref _DropConfigs, value); }
        }

        public DropConfig ActDropConfig
        {
            get => _ActDropConfig;
            set { SetProperty(ref _ActDropConfig, value); }
        }

        public List<string> PortList
        {
            get => _PortList;
            set { SetProperty(ref _PortList, value); }
        }

        public string SerialDialog
        {
            get => _SerialDialog;
            set { SetProperty(ref _SerialDialog, value); }
        }

        public int Passes
        {
            get => _Passes;
            set
            {
                value = value >= 0 ? value : 0;
                SetProperty(ref _Passes, value);
            }
        }

        public int Delay
        {
            get => _Delay;
            set
            {
                value = value >= 0 ? value : 0;
                SetProperty(ref _Delay, value);
            }
        }

        public int SelTabIdx
        {
            get => _SelTabIdx;
            set { SetProperty(ref _SelTabIdx, value); }
        }

        public ICommand ICLoadConf { get; set; }
        public ICommand ICDeleteConf { get; set; }
        public ICommand ICNewConf { get; set; }
        public ICommand ICSaveConf { get; set; }
        public ICommand ICCopyConf { get; set; }
        public ICommand ICScanPorts { get; set; }
        public ICommand ICSerialOpen { get; set; }
        public ICommand ICSerialClose { get; set; }
        public ICommand ICSendCmd { get; set; }
        public ICommand ICRunTasks { get; set; }
        public ICommand ICSetTasks { get; set; }
        public ICommand ICAddDevice { get; set; }
        public ICommand ICDelDevice { get; set; }
        public ICommand ICAddTask { get; set; }
        public ICommand ICDelTask { get; set; }
        public ICommand ICSetDuration { get; set; }
        public ICommand ICSetDeviceOn { get; set; }
        public ICommand ICSetDeviceOff { get; set; }




        public ViewModel()
        {
            ICLoadConf = new RelayCommand(LoadConf);
            ICDeleteConf = new RelayCommand(DeleteConf);
            ICNewConf = new RelayCommand(NewConf);
            ICSaveConf = new RelayCommand(SaveConf);
            ICCopyConf = new RelayCommand(CopyConf);
            ICScanPorts = new RelayCommand(ScanPorts);
            ICSerialOpen = new RelayCommand(OpenPort);
            ICSerialClose = new RelayCommand(ClosePort);
            ICSendCmd = new RelayCommand(SendCmd);
            ICRunTasks = new RelayCommand(RunTasks);
            ICSetTasks = new RelayCommand(SetTasks);
            ICAddDevice = new RelayCommand(AddDevice);
            ICDelDevice = new RelayCommand(DelDevice);
            ICAddTask = new RelayCommand(AddTask);
            ICDelTask = new RelayCommand(DelTask);
            ICSetDuration = new RelayCommand(SetDuration);
            ICSetDeviceOn = new RelayCommand(SetDeviceOn);
            ICSetDeviceOff = new RelayCommand(SetDeviceOff);

            SerialCom.NewSerialReading += UpdateSerialDialog;

            DBHandler.OpenConnection();
            UpdateDropConfigs();
            ScanPorts(null);
        }

        ~ViewModel()
        {
            DBHandler.CloseConnection();
        }

        private void UpdateDropConfigs()
        { DropConfigs = DBHandler.GetDropConfigs(); }

        private void LoadConf(object obj)
        {
            ActDropConfig = DBHandler.GetDropConfig((int)obj);
            SelTabIdx = 1;
        }

        private void DeleteConf(object obj)
        {
            DBHandler.DeleteDropConfig((int)obj);
            UpdateDropConfigs();
        }

        private void NewConf(object obj)
        {
            ActDropConfig = new DropConfig();
            ActDropConfig.Id = DBHandler.AddDropConfig(ActDropConfig);
            UpdateDropConfigs();
        }

        private void SaveConf(object obj)
        {
            DBHandler.UpdateDropConfig(ActDropConfig);
            UpdateDropConfigs();
        }

        private void CopyConf(object obj)
        {
            ActDropConfig.Id = DBHandler.AddDropConfig(ActDropConfig);
            for (int i = 0; i < ActDropConfig.Devices.Count(); i++)
            {
                ActDropConfig.Devices[i].DropConfigId = ActDropConfig.Id;
                ActDropConfig.Devices[i].Id = DBHandler.AddDevice(ActDropConfig.Devices[i]);
                for (int j = 0; j < ActDropConfig.Devices[i].DropTasks.Count() ; j++)
                {
                    ActDropConfig.Devices[i].DropTasks[j].DeviceId = ActDropConfig.Devices[i].Id;
                    ActDropConfig.Devices[i].DropTasks[j].Id = DBHandler.AddDropTask(ActDropConfig.Devices[i].DropTasks[j]);
                }
            }
            UpdateDropConfigs();
        }

        private void ScanPorts(object obj)
        {
            PortList = SerialCom.ListSerialPorts();
        }

        private void OpenPort(object obj)
        {
            int _retval = SerialCom.OpenPort((string)obj);
            switch (_retval)
            {
                case 0:
                    SerialDialog = "Port opened \n";
                    break;
                case 1:
                    SerialDialog += "Portname must begin with COM \n";
                    break;
                case 2:
                    SerialDialog += "there is already an open port \n";
                    break;
                default:
                    break;
            }
        }

        private void ClosePort(object obj)
        {
            SerialCom.ClosePort();
            SerialDialog += "Port closed \n";
        }

        private void SendCmd(object obj)
        {
            Enum.TryParse<SerialCom.BasicCMD>((string)obj, out SerialCom.BasicCMD _cmd);
            SerialCom.SendBasicCmd(_cmd);
        }

        private void RunTasks(object obj)
        {
            SerialCom.SendRunCmd(Passes, Delay);
        }

        private void SetTasks(object obj)
        {
            foreach (var device in ActDropConfig.Devices)
            {
                if (device.DevType == Device.EDevType.undef)
                    continue;
                if (device.DropTasks is null)
                    continue;
                if (device.DropTasks.Count < 1)
                    continue;
                SerialCom.SendTaskList(device.DevType, device.Pin, device.DropTasks.ToList());
            }
        }

        private void UpdateSerialDialog(object sender, SerialReadingEventArgs e)
        {
            SerialDialog += e.Reading;
        }

        private void AddDevice(object obj)
        {
            Enum.TryParse<Device.EDevType>((string)obj, out Device.EDevType _devType);
            Device device = new Device(ActDropConfig.Id, _devType);
            device.Id = DBHandler.AddDevice(device);
            ActDropConfig.Devices.Add(device);
        }

        private void DelDevice(object obj)
        {
            int devId = (obj as Device).Id;
            DBHandler.DelDevice(devId);
            ActDropConfig.Devices.Remove((Device)obj);
        }

        private void AddTask(object obj)
        {
            Device device = obj as Device;
            int devIdx = ActDropConfig.Devices.IndexOf(device);
            DropTask dropTask = new DropTask(device.Id, 0, device.Duration);
            dropTask.Id = DBHandler.AddDropTask(dropTask);
            ActDropConfig.Devices[devIdx].DropTasks.Add(dropTask);
        }

        private void DelTask(object obj)
        {
            Device device = obj as Device;
            int devIdx = ActDropConfig.Devices.IndexOf(device);
            int taskCount = ActDropConfig.Devices[devIdx].DropTasks.Count();
            if (taskCount < 1)
            { return; }
            DBHandler.DelDropTask(ActDropConfig.Devices[devIdx].DropTasks[taskCount - 1].Id);
            ActDropConfig.Devices[devIdx].DropTasks.RemoveAt(taskCount - 1);
        }

        private void SetDuration(object obj)
        {
            Device tmpDevice = obj as Device;
            int tmpIdx = ActDropConfig.Devices.IndexOf(tmpDevice);
            for (int i = 0; i < ActDropConfig.Devices[tmpIdx].DropTasks.Count(); i++)
            {
                ActDropConfig.Devices[tmpIdx].DropTasks[i].Duration = ActDropConfig.Devices[tmpIdx].Duration;
            }
        }

        private void SetDeviceOn(object obj)
        { SerialCom.SendHighLowCmd(true, (obj as Device).Pin); }
        private void SetDeviceOff(object obj)
        { SerialCom.SendHighLowCmd(false, (obj as Device).Pin); }

    }
}
