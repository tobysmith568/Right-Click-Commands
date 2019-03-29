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

        public void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
