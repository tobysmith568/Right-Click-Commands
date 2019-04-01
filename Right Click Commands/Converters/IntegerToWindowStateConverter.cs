using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Right_Click_Commands.Converters
{
    public class IntegerToWindowStateConverter : ConverterBase<IntegerToWindowStateConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            WindowState result;

            try
            {
                result = (WindowState)value;
            }
            catch
            {
                result = 0;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
