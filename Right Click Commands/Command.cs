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
        protected Action<object> parameterizedAction = null;
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
            DoExecute(parameter);
        }

        //  Constructors
        //  ============

        public Command(Action action, bool canExecute = true)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public Command(Action<object> parameterizedAction, bool canExecute = true)
        {
            this.parameterizedAction = parameterizedAction;
            this.canExecute = canExecute;
        }

        //  Methods
        //  =======

        protected void InvokeAction(object param)
        {
            Action theAction = action;
            Action<object> theParameterizedAction = parameterizedAction;
            if (theAction != null)
                theAction();
            else
                theParameterizedAction?.Invoke(param);
        }

        public virtual void DoExecute(object param)
        {
            InvokeAction(param);
        }
    }
}
