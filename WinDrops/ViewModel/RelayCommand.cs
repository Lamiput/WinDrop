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
using System.Windows.Input;


namespace WinDrops.ViewModel
{
    class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Predicate<object> canExecute;
        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<object> _execute) : this(_execute, DefaultCanExecute) { }
        public RelayCommand(Action<object> _execute, Predicate<object> _canExecute)
        {
            execute = _execute ?? throw new ArgumentNullException("execute");
            canExecute = _canExecute ?? throw new ArgumentNullException("canExecute");
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object _parameter)
        {
            return canExecute != null && canExecute(_parameter);
        }

        public void Execute(object _parameter)
        {
            execute(_parameter);
        }

        public void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChangedInternal;
            if (handler != null)
            {
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ => { return; };
        }

        private static bool DefaultCanExecute(object _parameter)
        {
            return true;
        }
    }
}
