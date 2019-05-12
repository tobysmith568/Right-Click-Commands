using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Right_Click_Commands.WPF.Converters
{
    public class NullableToVisibility : ConverterBase<NullableToVisibility>
    {
        //  Methods
        //  =======

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                throw new ArgumentException("The given value must be a Visibility");

            switch ((Visibility)value)
            {
                case Visibility.Visible:
                    return new object();
                default:
                    return null;
            }
        }
    }
}
