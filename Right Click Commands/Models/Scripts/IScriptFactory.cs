﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.Scripts
{
    public interface IScriptFactory<T>
    {
        //  Methods
        //  =======

        /// <exception cref="InvalidDataException"></exception>
        IScriptConfig Generate(T input, MenuLocation menuLocation);
    }
}