using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Right_Click_Commands.Views
{
    /// <summary>
    /// Interaction logic for CommandConfig.xaml
    /// </summary>
    public partial class CommandConfig : UserControl, INotifyPropertyChanged
    {
        //  Variables
        //  =========

        private string label = "";
        private string icon;

        //  Properties
        //  ==========

        public string Label
        {
            get => label;
            set
            {
                if (label != value)
                {
                    label = value;
                    RaisePropertyChanged("Label");
                }
            }
        }

        public string Icon
        {
            get => icon;
            set
            {
                if (icon != value)
                {
                    icon = value;
                    RaisePropertyChanged("Icon");
                }
            }
        }

        //  Constructors
        //  ============

        public CommandConfig()
        {
            InitializeComponent();
        }

        //  Events
        //  ======

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
