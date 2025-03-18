using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GetStartedApp.Command
{
    public class TestCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            var o = (dynamic)parameter;
            if (string.IsNullOrEmpty(o.value1) || string.IsNullOrEmpty(o.value2))
            {
                return false;
            }
            
            return true;
        }

        public void Execute(object? parameter)
        {
            
        }
    }
}
