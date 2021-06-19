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

using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using WinDrops.Model;


namespace WinDrops.View
{
    /// <summary>
    /// Interaktionslogik für UCDevice.xaml
    /// </summary>
    public partial class UCDevice : UserControl
    {
        public UCDevice()
        {
            InitializeComponent();
        }
    }

    public class DevicetypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Device.EDevType)value)
            {
                case Device.EDevType.undef:
                    return new SolidColorBrush(Colors.LightGray);
                case Device.EDevType.Valve:
                    return new SolidColorBrush(Colors.LightBlue);
                case Device.EDevType.Flash:
                    return new SolidColorBrush(Colors.LightYellow);
                case Device.EDevType.Camera:
                    return new SolidColorBrush(Colors.LightGreen);
                default:
                    return new SolidColorBrush(Colors.LightGray);
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Device.EDevType.undef;
        }
    }

}
