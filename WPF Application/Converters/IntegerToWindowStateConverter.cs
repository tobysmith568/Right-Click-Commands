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
    public class IntegerToWindowStateConverter : ConverterBase<IntegerToWindowStateConverter>
    {
        //  Methods
        //  =======

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case 1:
                    return WindowState.Minimized;
                case 2:
                    return WindowState.Maximized;
                default:
                    return WindowState.Normal;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is WindowState))
                throw new ArgumentException("The given value must be a WindowState");

            int result;

            try
            {
                result = (int)value;
            }
            catch
            {
                result = 0;
            }

            return result;
        }
    }
}
