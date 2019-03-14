using Right_Click_Commands.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.ViewModels
{
    public class MainWindowViewModel
    {
        //  Properties
        //  ==========

        public ObservableCollection<CommandConfig> CommandConfigs { get; set; }

        //  Constructors
        //  ============

        public MainWindowViewModel()
        {
            CommandConfigs = new ObservableCollection<CommandConfig>()
            {
                new CommandConfig()
                {
                    Label = "Child item #1",
                    Children = new ObservableCollection<CommandConfig>()
                    {
                        new CommandConfig()
                        {
                            Label = "Child item #1.1"
                        },
                        new CommandConfig()
                        {
                            Label = "Child item #1.2"
                        }
                    }
                },
                new CommandConfig()
                {
                    Label = "Child item #2"
                }
            };            
        }
    }
}
