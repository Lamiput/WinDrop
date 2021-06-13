using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinDrops.Model;

namespace WinDrops.ViewModel
{
    class VMSetupController
    {
        public VMSerialCom VmSerialCom { get; private set; }
        public VMDropConfig VmDropConfig { get; private set; }
        public DropConfig DropConfig;


        public VMSetupController()
        {
            DropConfig = new DropConfig();
            VmSerialCom = new VMSerialCom(ref DropConfig);
            VmDropConfig = new VMDropConfig(ref DropConfig);

        }
    }
}
