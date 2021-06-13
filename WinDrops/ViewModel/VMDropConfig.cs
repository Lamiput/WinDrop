using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using WinDrops.Model;


namespace WinDrops.ViewModel
{
    class VMDropConfig: ViewModelBase
    {
        private DropConfig _DropConfig;

        private ObservableCollection<VMDevice> _Devices = new ObservableCollection<VMDevice>();
        public ObservableCollection<VMDevice> Devices
        {
            get => _Devices;
            set { SetProperty(ref _Devices, value); }
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                if (SetProperty(ref _Name, value))
                { _DropConfig.Name = value; }
            }
        }

        private string _Description;
        public string Description
        {
            get => _Description;
            set
            {
                if (SetProperty(ref _Description, value))
                { _DropConfig.Description = value; }
            }
        }

        public ICommand ICAddDevice { get; set; }
        private void AddDevice(object obj)
        {
            Enum.TryParse<Device.EDevType>((string)obj, out Device.EDevType _devType);
            _DropConfig.Devices.Add(new Device(_devType));
            Device tmpDevice = _DropConfig.Devices.Last();
            Devices.Add(new VMDevice(ref tmpDevice));
        }

        public ICommand ICDelSelDevices { get; set; }
        private void DelSelDevices(object obj)
        {
            foreach (VMDevice dev in Devices.Where(x => x.Marked).ToList())
            {
                _DropConfig.Devices.Remove(dev.Device);
                Devices.Remove(dev);
            }
        }


        public VMDropConfig(ref DropConfig dropConfig)
        {
            _DropConfig = dropConfig;

            ICAddDevice = new RelayCommand(AddDevice);
            ICDelSelDevices = new RelayCommand(DelSelDevices);

            UpdateDropConfig();
        }

        private void UpdateDropConfig()
        {
            Name = _DropConfig.Name;
            Description = _DropConfig.Description;
        }

    }
}
