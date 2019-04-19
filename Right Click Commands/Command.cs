using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Right_Click_Commands
{
    public class Command : Command<object>
    {
        //  Constructors
        //  ============

        public Command(Action action, bool canExecute = true) : base(action, canExecute)
        {
        }

        public Command(Action<object> parameterizedAction, bool canExecute = true) : base(parameterizedAction, canExecute)
        {
        }
    }

    public class Command<T> : ICommand// where T : class
    {
        //  Variables
        //  =========

        protected Action action = null;
        protected Action<T> parameterizedAction = null;
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

        //  Constructors
        //  ============

        public Command(Action action, bool canExecute = true)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public Command(Action<T> parameterizedAction, bool canExecute = true)
        {
            this.parameterizedAction = parameterizedAction;
            this.canExecute = canExecute;
        }

        //  Methods
        //  =======

        bool ICommand.CanExecute(object parameter)
        {
            return canExecute;
        }

        void ICommand.Execute(object parameter)
        {
            DoExecute(parameter);
        }

        public virtual void DoExecute(object param)
        {
            InvokeAction((T)param);
        }

        protected void InvokeAction(T param)
        {
            Action theAction = action;
            Action<T> theParameterizedAction = parameterizedAction;
            if (theAction != null)
                theAction();
            else
                theParameterizedAction?.Invoke(param);
        }
    }
}
