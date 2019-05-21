using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Right_Click_Commands.Utils
{
    public static class EnumUtils
    {
        public static T ToEnum<T>(this string value) where T : struct
        {
            if (Enum.TryParse(value, true, out T result))
            {
                return result;
            }
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
