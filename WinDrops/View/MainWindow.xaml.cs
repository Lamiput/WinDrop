using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace WinDrops.View
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
