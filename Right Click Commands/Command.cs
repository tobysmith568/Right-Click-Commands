using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Right_Click_Commands
{
    public class Command : ICommand
    {
        //  Variables
        //  =========

        protected Action action = null;
        protected Action<string> parameterizedAction = null;
        public event EventHandler CanExecuteChanged;

        private bool canExecute = false;

        //  Properties
        //  ==========

        public bool CanExecute
        {
            get => canExecute;
            set
            {
                if (canExecute != value)
                {
                    canExecute = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        bool ICommand.CanExecute(object parameter)
        {
            return canExecute;
        }

        void ICommand.Execute(object parameter)
        {
            DoExecute(parameter?.ToString());
        }

        //  Constructors
        //  ============

        public Command(Action action, bool canExecute = true)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public Command(Action<string> parameterizedAction, bool canExecute = true)
        {
            this.parameterizedAction = parameterizedAction;
            this.canExecute = canExecute;
        }

        //  Methods
        //  =======

        protected void InvokeAction(string param)
        {
            Action theAction = action;
            Action<string> theParameterizedAction = parameterizedAction;
            if (theAction != null)
                theAction();
            else
                theParameterizedAction?.Invoke(param);
        }

        public virtual void DoExecute(string param)
        {
            InvokeAction(param);
        }
    }
}
