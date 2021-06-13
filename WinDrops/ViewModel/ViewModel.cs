using WinDrops.Utils;
using WinDrops.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WinDrops.View;

namespace WinDrops.ViewModel
{
    class ViewModel : ViewModelBase
    {
        public ObservableCollection<TabItem> TabItems { get; set; }
        private readonly VMManageConfigs _VMManageConfigs;
        private readonly VMSetupController _VMSetupController;

        public ViewModel()
        {
            _VMManageConfigs = new VMManageConfigs();
            _VMSetupController = new VMSetupController();

            TabItems = new ObservableCollection<TabItem>
            {
                new TabItem {Content = new UCManageConfigs() { DataContext = _VMManageConfigs }, Header = "Manage Configurations" },
                new TabItem {Content = new UCSetupController() { DataContext = _VMSetupController }, Header = "Setup Controller"}
            };
        }

        ~ViewModel()
        { }


    }
}
