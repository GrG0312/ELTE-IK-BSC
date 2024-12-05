using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BlackHole.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> execute;
        private readonly Func<Object?, bool>? canExcecute;

        public DelegateCommand(Action<Object?> execute) : this(null, execute) { }
        public DelegateCommand(Func<Object?, bool>? canExecute, Action<Object?> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = execute;
            this.canExcecute = canExecute;
        }
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter)
        {
            return this.canExcecute == null ? true : canExcecute(parameter);
        }
        public void Execute(object? parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled!");
            }
            this.execute(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
