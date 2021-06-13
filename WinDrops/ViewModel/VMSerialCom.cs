using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using WinDrops.Model;


namespace WinDrops.ViewModel
{
    class VMSerialCom : ViewModelBase
    {
        private readonly SerialCom serialCom = new SerialCom();
        private readonly DropConfig _DropConfig;

        private List<string> _PortList;
        public List<string> PortList
        {
            get => _PortList;
            set { SetProperty(ref _PortList, value); }
        }
        private int _PortIdx = 0;
        public int PortIdx
        {
            get => _PortIdx;
            set { SetProperty(ref _PortIdx, value); }
        }

        private string _SerialDialog = "";
        public string SerialDialog
        {
            get => _SerialDialog;
            set { SetProperty(ref _SerialDialog, value); }
        }

        private int _Passes = 0;
        public int Passes
        {
            get => _Passes;
            set {
                value = value >= 0 ? value : 0;
                SetProperty(ref _Passes, value);
            }
        }

        private int _Delay = 0;
        public int Delay
        {
            get => _Delay;
            set {
                value = value >= 0 ? value : 0;
                SetProperty(ref _Delay, value);
            }
        }

        public ICommand ICScanPorts { get; set; }
        private void ScanPorts(object obj)
        {
            PortList = SerialCom.ListSerialPorts();
            PortIdx = 0;
        }

        public ICommand ICSerialOpen { get; set; }
        private void OpenPort(object obj)
        {
            int _retval = serialCom.OpenPort((string)obj);
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

        public ICommand ICSerialClose { get; set; }
        private void ClosePort(object obj)
        {
            serialCom.ClosePort();
            SerialDialog += "Port closed \n";
        }

        public ICommand ICSendCmd { get; set; }
        private void SendCmd(object obj)
        {
            Enum.TryParse<SerialCom.BasicCMD>((string)obj, out SerialCom.BasicCMD _cmd);
            serialCom.SendBasicCmd(_cmd);
        }

        public ICommand ICRunTasks { get; set; }
        private void RunTasks(object obj)
        {
            serialCom.SendRunCmd(Passes, Delay);
        }

        public ICommand ICSetTasks { get; set; }
        private void SetTasks(object obj)
        {
            foreach (var device in _DropConfig.Devices)
            {
                if (device.DevType == Device.EDevType.undef)
                    continue;
                if (device.DropTasks is null)
                    continue;
                if (device.DropTasks.Count < 1)
                    continue;
                serialCom.SendTaskList(device.DevType, device.Pin, device.DropTasks.ToList());
            }
        }

        public VMSerialCom(ref DropConfig dropConfig)
        {
            _DropConfig = dropConfig;

            ICScanPorts = new RelayCommand(ScanPorts);
            ICSerialOpen = new RelayCommand(OpenPort);
            ICSerialClose = new RelayCommand(ClosePort);
            ICSendCmd = new RelayCommand(SendCmd);
            ICRunTasks = new RelayCommand(RunTasks);
            ICSetTasks = new RelayCommand(SetTasks);

            serialCom.NewSerialReading += UpdateSerialDialog;
        }

        private void UpdateSerialDialog(object sender, SerialReadingEventArgs e)
        {
            SerialDialog += e.Reading;
        }

    }
}
