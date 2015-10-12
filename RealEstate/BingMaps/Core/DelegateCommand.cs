using System;
using System.Windows.Input;

namespace RealEstate.BingMaps.Core
{
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> _execute;

        public DelegateCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute.Invoke((T)parameter);
        }
    }
}
