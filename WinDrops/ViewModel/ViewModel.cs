using WinDrops.Utils;
using WinDrops.Model;


namespace WinDrops.ViewModel
{
    class ViewModel : ViewModelBase
    {
        public VMSerialCom VmSerialCom { get; private set; }
        public VMDropConfig VmDropConfig { get; private set; }

        public DropConfig DropConfig;


        public ViewModel()
        {
            DBHandler.OpenConnection();

            DropConfig = new DropConfig();

            VmSerialCom = new VMSerialCom(ref DropConfig);
            VmDropConfig = new VMDropConfig(ref DropConfig);
        }

        ~ViewModel()
        {
            DBHandler.CloseConnection();
        }


    }
}
