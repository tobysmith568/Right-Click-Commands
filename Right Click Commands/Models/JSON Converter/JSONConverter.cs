﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.JSON_Converter
{
    public class JSONConverter : IJSONConverter
    {
        //  Variables
        //  =========

        public T FromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string ToJson<T>(T model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
