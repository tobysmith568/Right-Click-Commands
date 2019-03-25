using Right_Click_Commands.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Right_Click_Commands.ViewModels
{
    public class MainWindowViewModel
    {
        //  Properties
        //  ==========

        public ObservableCollection<CommandConfig> CommandConfigs { get; set; }
        public Command SimpleCommand { get; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {
            CommandConfigs = new ObservableCollection<CommandConfig>()
            {
                new CommandConfig()
                {
                    Label = "Item #1"
                },
                new CommandConfig()
                {
                    Label = "Item #2"
                }
            };
            
            SimpleCommand = new Command(DoSimpleCommand);
        }

        private void DoSimpleCommand()
        {
            MessageBox.Show("Hello");
        }
    }
}
