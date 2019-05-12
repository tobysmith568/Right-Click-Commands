using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        //  Events
        //  ======

        public event PropertyChangedEventHandler PropertyChanged;

        //  Methods
        //  =======

        protected void PropertyChanging<T>(T value, ref T variable, string propertyName, params string[] otherPropertyNames)
        {
            if (!EqualityComparer<T>.Default.Equals(value, variable))
            {
                variable = value;
                RaisePropertyChanged(propertyName);

                if (otherPropertyNames != null)
                {
                    foreach (string property in otherPropertyNames)
                    {
                        RaisePropertyChanged(property);
                    }
                }
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
