﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Settings
{
    public interface ISettings
    {
        //  Properties
        //  ==========

        string ScriptLocation { get; }

        //  Methods
        //  =======

        void SaveAll();
    }
}
