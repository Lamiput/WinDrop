using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinDrops.View
{
    /// <summary>
    /// Interaktionslogik für UCSetupController.xaml
    /// </summary>
    public partial class UCSetupController : UserControl
    {
        public UCSetupController()
        {
            InitializeComponent();
        }
        private void ScrollToLastLine(object sender, DataTransferEventArgs e)
        {
            if (sender is TextBox box)
            { box.ScrollToEnd(); }
        }
    }
}
