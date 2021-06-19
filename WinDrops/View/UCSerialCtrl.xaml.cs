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

using System.Windows.Controls;
using System.Windows.Data;

namespace WinDrops.View
{
    /// <summary>
    /// Interaktionslogik für UCSerialCtrl.xaml
    /// </summary>
    public partial class UCSerialCtrl : UserControl
    {
        public UCSerialCtrl()
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
