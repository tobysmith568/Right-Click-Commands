using Right_Click_Commands.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.ViewModels
{
    public class MenuItem
    {
        public string Title { get; set; }
        public ObservableCollection<MenuItem> Items { get; set; }

        public MenuItem()
        {
            Items = new ObservableCollection<MenuItem>();
        }
    }

    public class MainWindowViewModel
    {
        //  Properties
        //  ==========

        public ObservableCollection<MenuItem> CommandConfigs { get; set; }

        public MainWindowViewModel()
        {
            CommandConfigs = new ObservableCollection<MenuItem>()
            {
                new MenuItem()
                {
                    Title = "Child item #1",
                    Items = new ObservableCollection<MenuItem>()
                    {
                        new MenuItem()
                        {
                            Title = "Child item #1.1"
                        },
                        new MenuItem()
                        {
                            Title = "Child item #1.2"
                        }
                    }
                },
                new MenuItem()
                {
                    Title = "Child item #2"
                }
            };            
        }
    }
}
