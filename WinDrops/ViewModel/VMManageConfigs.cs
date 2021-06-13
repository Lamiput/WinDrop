using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinDrops.Model;
using WinDrops.Utils;

namespace WinDrops.ViewModel
{
    class VMManageConfigs : ViewModelBase
    {
        private List<DropConfig> _DropConfigs;

        public List<DropConfig> DropConfigs
        {
            get => _DropConfigs;
            set { SetProperty(ref _DropConfigs, value); }
        }

        public ICommand ICNewConf { get; set; }
        public ICommand ICLoadConf { get; set; }
        public ICommand ICDeleteConf { get; set; }

        public VMManageConfigs()
        {
            DBHandler.OpenConnection();
            ICNewConf = new RelayCommand(NewConf);
            ICLoadConf = new RelayCommand(LoadConf);
            ICDeleteConf = new RelayCommand(DeleteConf);

            DropConfigs = DBHandler.GetDropConfigs();
        }

        ~VMManageConfigs()
        {
            DBHandler.CloseConnection();
        }

        private void NewConf(object obj)
        {
            DBHandler.AddDropConfig();
            DropConfigs = DBHandler.GetDropConfigs();
        }

        private void LoadConf(object obj)
        { }

        private void DeleteConf(object obj)
        {
            DBHandler.DeleteDropConfig((DropConfig)obj);
            DropConfigs = DBHandler.GetDropConfigs();
        }
    }
}
