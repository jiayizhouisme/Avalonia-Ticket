using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GetStartedApp.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private event EventHandler CanExecuteChangedInternal;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CanExecuteChangedInternal -= value;
            }
        }
        public RelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            var can = _canExecute == null || _canExecute();
            Debug.WriteLine($"CanExecute RegisterCommand: {can}");
            return can;
        }
        public async void Execute(object parameter)
        {
            try
            {
                await _execute();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error executing command: {ex.Message}");
            }
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
        }
    }
}