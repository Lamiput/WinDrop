using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using WinDrops.Model;


namespace WinDrops.ViewModel
{
    class VMDevice : ViewModelBase
    {
        private Device _Device;
        public Device Device
        {
            get => _Device;
            private set { SetProperty(ref _Device, value); }
        }

        private string _Name;
        public string Name
        {
            get => _Name;
            set {
                if (SetProperty(ref _Name, value))
                    Device.Name = value;
            }
        }

        private bool _Marked;
        public bool Marked
        {
            get => _Marked;
            set { SetProperty(ref _Marked, value); }
        }

        private int _Pin;
        public int Pin
        {
            get => _Pin;
            set {
                if (SetProperty(ref _Pin, value))
                    Device.Pin = value;
            }
        }

        private int _Duration;
        public int Duration
        {
            get => _Duration;
            set {
                if (SetProperty(ref _Duration, value))
                    Device.Duration = value;
            }
        }

        private ObservableCollection<DropTask> _DropTasks;
        public ObservableCollection<DropTask> DropTasks
        {
            get => _DropTasks;
            set { SetProperty(ref _DropTasks, value); }
        }

        private Brush _BGBrush;
        public Brush BGBrush
        {
            get => _BGBrush;
            set { SetProperty(ref _BGBrush, value); }
        }

        public ICommand ICAddTask { get; set; }
        private void AddTask(object obj)
        {
            DropTask tmpTask = new DropTask(0, Duration);
            DropTasks.Add(tmpTask);
            _Device.DropTasks.Add(tmpTask);
        }

        public ICommand ICRemTask { get; set; }
        private void RemTask(object obj)
        {
            if (DropTasks.Count > 0)
                DropTasks.RemoveAt(DropTasks.Count - 1);
            if (_Device.DropTasks.Count > 0)
                _Device.DropTasks.RemoveAt(_Device.DropTasks.Count - 1);
        }

        public ICommand ICSetDuration { get; set; }
        private void SetDuration(object obj)
        {
            ObservableCollection<DropTask> tmpTasks = new ObservableCollection<DropTask>();
            for (int i = 0; i < DropTasks.Count; i++)
            {
                tmpTasks.Add(new DropTask { StartTime = DropTasks[i].StartTime, Duration=Duration });
            }
            DropTasks = new ObservableCollection<DropTask>(tmpTasks);
            _Device.DropTasks = DropTasks.ToList();
        }

        public VMDevice(ref Device device)
        {

             switch (device.DevType)
            {
                case Device.EDevType.undef:
                    BGBrush = new SolidColorBrush(Colors.LightGray);
                    break;
                case Device.EDevType.Valve:
                    BGBrush = new SolidColorBrush(Colors.LightBlue);
                    break;
                case Device.EDevType.Flash:
                    BGBrush = new SolidColorBrush(Colors.LightYellow);
                    break;
                case Device.EDevType.Camera:
                    BGBrush = new SolidColorBrush(Colors.LightGreen);
                    break;
                default:
                    break;
            }

            _Device = device;
            ICAddTask = new RelayCommand(AddTask);
            ICRemTask = new RelayCommand(RemTask);
            ICSetDuration = new RelayCommand(SetDuration);

            Update();
        }


        private void Update()
        {
            Name = _Device.Name;
            Pin = _Device.Pin;
            Duration = _Device.Duration;
            DropTasks = new ObservableCollection<DropTask>(_Device.DropTasks);
        }

    }
}
