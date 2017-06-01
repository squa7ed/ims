using System;
using System.Diagnostics;
using System.Windows.Input;

namespace IMS.Common
{
    public class RelayCommand : ICommand
    {
        private Func<bool> canExecute;
        private Action execute;

        public RelayCommand(Action action, Func<bool> predicate = null)
        {
            execute = action ?? throw new ArgumentNullException("action");
            canExecute = predicate;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        public void Execute(object parameter)
        {
            execute();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Predicate<T> canExecute;
        private Action<T> execute;

        public RelayCommand(Action<T> action, Predicate<T> predicate = null)
        {
            execute = action ?? throw new ArgumentNullException("action");
            canExecute = predicate;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null ? true : canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
    }
}
