using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Models.JSON_Converter
{
    public interface IJSONConverter
    {
        T FromJson<T>(string json);
        string ToJson<T>(T model);
    }
}
