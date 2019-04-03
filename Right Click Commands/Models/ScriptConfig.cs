﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models
{
    public class ScriptConfig : ViewModelBase
    {
        private string name;
        private string label = "";
        private string icon;

        //  Properties
        //  ==========

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

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

        public ScriptConfig()
        {
        }
    }
}