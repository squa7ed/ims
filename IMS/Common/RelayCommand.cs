using System;
using System.Windows.Input;

namespace IMS.Common
{
    public class RelayCommand : ICommand
    {
        private Predicate<object> canExecute;
        private Action<object> execute;

        public RelayCommand(Action<object> action, Predicate<object> predicate = null)
        {
            execute = action ?? throw new ArgumentNullException("action");
            canExecute = predicate;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter = null)
        {
            return canExecute == null ? true : canExecute(parameter);
        }

        public void Execute(object parameter = null)
        {
            execute(parameter);
        }
    }
}
