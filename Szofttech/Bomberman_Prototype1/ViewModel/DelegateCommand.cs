using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bomberman_Prototype1.ViewModel
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<Object?> execute;
        private readonly Func<Object?, Boolean>? canExecute;
        public DelegateCommand(Action<Object?> execute) : this(null, execute) { }
        public DelegateCommand(Func<Object?, Boolean>? canExecute, Action<Object?> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;
        public Boolean CanExecute(Object? parameter)
        {
            return canExecute == null ? true : canExecute(parameter);
        }
        public void Execute(Object? parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("Command execution is disabled.");
            }
            execute(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
