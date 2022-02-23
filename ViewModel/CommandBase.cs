using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VibrationTestBench.ViewModel
{
    public class CommandBase : ICommand//事件Class
    {
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            if (this.CanExecuteFunc == null)
            {
                return true;
            }
            else
            {
                return this.CanExecuteFunc(parameter);
            }
        }

        public void Execute(object parameter)
        {
            if (this.ExecuteAction == null)
            {
                return;
            }
            else
            {
                this.ExecuteAction(parameter);
            }
        }
        public Action<object> ExecuteAction { get; set; }
        public Func<object, bool> CanExecuteFunc { get; set; }
    }
}
