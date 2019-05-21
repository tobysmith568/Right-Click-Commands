using Right_Click_Commands.Utils;
using Right_Click_Commands.WPF.Models.Scripts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Right_Click_Commands.WPF.Converters
{
    public class ScriptTypeToStringConveter : ConverterBase<ScriptTypeToStringConveter>
    {
        //  Methods
        //  =======

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ScriptType))
                throw new ArgumentException("The given value must be a ScriptType");

            return ((ScriptType)value).ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
